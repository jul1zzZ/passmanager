using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using pass.Models;

namespace pass.Services;

public static class BackupService
{
    // ---------- Экспорт ----------
    public static async Task<string> ExportAsync(string password, IEnumerable<Account> accounts)
    {
        var json = JsonSerializer.Serialize(accounts, new JsonSerializerOptions
        {
            WriteIndented = true
        });

        var encrypted = EncryptString(json, password);

#if ANDROID
        string downloadsPath = Android.OS.Environment.GetExternalStoragePublicDirectory(
            Android.OS.Environment.DirectoryDownloads).AbsolutePath;
#elif WINDOWS
        string downloadsPath = Path.Combine(
            Environment.GetFolderPath(Environment.SpecialFolder.UserProfile),
            "Downloads");
#else
        string downloadsPath = FileSystem.Current.AppDataDirectory;
#endif

        var fileName = $"backup_{DateTime.Now:yyyyMMdd_HHmmss}.enc";
        var path = Path.Combine(downloadsPath, fileName);

        await File.WriteAllBytesAsync(path, encrypted);

        return path;
    }

    // ---------- Импорт ----------
    public static async Task<List<Account>> ImportAsync(string password, Stream stream)
    {
        using var ms = new MemoryStream();
        await stream.CopyToAsync(ms);
        var data = ms.ToArray();

        try
        {
            var json = DecryptString(data, password);
            return JsonSerializer.Deserialize<List<Account>>(json) ?? new List<Account>();
        }
        catch (CryptographicException)
        {
            throw new Exception("Неверный пароль или повреждённый файл.");
        }
    }

    private static byte[] EncryptString(string plainText, string password)
    {
        using var aes = Aes.Create();
        var key = DeriveKey(password, aes.KeySize / 8);
        aes.Key = key;
        aes.GenerateIV();

        using var encryptor = aes.CreateEncryptor();
        using var ms = new MemoryStream();
        ms.Write(aes.IV, 0, aes.IV.Length);
        using (var cs = new CryptoStream(ms, encryptor, CryptoStreamMode.Write))
        using (var sw = new StreamWriter(cs, Encoding.UTF8))
        {
            sw.Write(plainText);
        }
        return ms.ToArray();
    }

    private static string DecryptString(byte[] cipherData, string password)
    {
        using var aes = Aes.Create();
        var key = DeriveKey(password, aes.KeySize / 8);
        aes.Key = key;

        using var ms = new MemoryStream(cipherData);
        var iv = new byte[aes.BlockSize / 8];
        ms.Read(iv, 0, iv.Length);
        aes.IV = iv;

        using var decryptor = aes.CreateDecryptor();
        using var cs = new CryptoStream(ms, decryptor, CryptoStreamMode.Read);
        using var sr = new StreamReader(cs, Encoding.UTF8);
        return sr.ReadToEnd();
    }

    private static byte[] DeriveKey(string password, int keyBytes)
    {
        var salt = Encoding.UTF8.GetBytes("pass_manager_salt");
        using var pbkdf2 = new Rfc2898DeriveBytes(password, salt, 100000, HashAlgorithmName.SHA256);
        return pbkdf2.GetBytes(keyBytes);
    }
}
