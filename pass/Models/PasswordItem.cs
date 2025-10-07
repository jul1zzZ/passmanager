using System.ComponentModel;
using System.Runtime.CompilerServices;
using SQLite;

namespace pass.Models;

public class PasswordItem : INotifyPropertyChanged
{
    [PrimaryKey, AutoIncrement]
    public int Id { get; set; }

    public string Service { get; set; }
    public string Login { get; set; }
    public string Password { get; set; }

    private bool isPasswordHidden = true;
    public bool IsPasswordHidden
    {
        get => isPasswordHidden;
        set
        {
            if (isPasswordHidden != value)
            {
                isPasswordHidden = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(DisplayPassword));
            }
        }
    }

    public string DisplayPassword => IsPasswordHidden ? "••••••" : Password;

    public event PropertyChangedEventHandler PropertyChanged;
    protected void OnPropertyChanged([CallerMemberName] string propertyName = "")
        => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
}
