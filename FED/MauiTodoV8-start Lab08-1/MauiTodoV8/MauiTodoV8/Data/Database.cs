using MauiTodoV8.Models;
using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MauiTodoV8.Data
{
    internal class Database
    {
        private readonly SQLiteAsyncConnection _connection;

        public Database()
        {
            var dataDir = FileSystem.AppDataDirectory;
            var databasePath = Path.Combine(dataDir, "MauiTodoV8.db");

            //string _dbEncryptionKey = SecureStorage.GetAsync("dbKey").Result;

            //if (string.IsNullOrEmpty(_dbEncryptionKey))
            //{
            //    Guid g = new Guid();
            //    _dbEncryptionKey = g.ToString();
            //    SecureStorage.SetAsync("dbKey", _dbEncryptionKey);
            //}

            //var dbOptions = new SQLiteConnectionString(databasePath, true, key: _dbEncryptionKey);
            var dbOptions = new SQLiteConnectionString(databasePath, true);

            _connection = new SQLiteAsyncConnection(dbOptions);

            _ = Initialise();
        }

        private async Task Initialise()
        {
            await _connection.CreateTableAsync<TodoItem>();
        }

        public async Task<List<TodoItem>> GetTodos()
        {
            return await _connection.Table<TodoItem>().ToListAsync();
        }

        public async Task<TodoItem> GetTodo(int id)
        {
            var query = _connection.Table<TodoItem>().Where(t => t.Id == id);

            return await query.FirstOrDefaultAsync();
        }

        public async Task<int> AddTodo(TodoItem item)
        {
            return await _connection.InsertAsync(item);
        }

        public async Task<int> DeleteTodo(TodoItem item)
        {
            return await _connection.DeleteAsync(item);
        }

        public async Task<int> UpdateTodo(TodoItem item)
        {
            return await _connection.UpdateAsync(item);
        }
    }
}
