using pass.Models;
using SQLite;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace pass.Services
{
    public class DatabaseService
    {
        private readonly SQLiteAsyncConnection _db;

        public DatabaseService(string dbPath)
        {
            _db = new SQLiteAsyncConnection(dbPath);
            _db.CreateTableAsync<Account>().Wait();
            _db.CreateTableAsync<Category>().Wait(); // создаем таблицу категорий
        }

        // ------------------ Аккаунты ------------------
        public Task<List<Account>> GetAccountsAsync()
            => _db.Table<Account>().OrderByDescending(a => a.CreatedAt).ToListAsync();

        public Task<int> SaveAccountAsync(Account account)
            => account.Id != 0 ? _db.UpdateAsync(account) : _db.InsertAsync(account);

        public Task<int> DeleteAccountAsync(Account account)
            => _db.DeleteAsync(account);

        // ------------------ Категории ------------------
        public Task<List<Category>> GetCategoriesAsync()
            => _db.Table<Category>().OrderBy(c => c.Name).ToListAsync();

        public Task<int> SaveCategoryAsync(Category category)
            => category.Id != 0 ? _db.UpdateAsync(category) : _db.InsertAsync(category);

        public Task<int> DeleteCategoryAsync(Category category)
            => _db.DeleteAsync(category);
    }
}
