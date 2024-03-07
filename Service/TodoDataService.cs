using TodoApi.Models;
using TodoApi.Interfaces;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using System.Data.SQLite;
using Microsoft.Extensions.Logging;
using Dapper;                           // Naudojama QueryAsync, QueryFirstOrDefaultAsync, QuerySingleAsync, ExecuteAsync
using System.ComponentModel.Design;
using System.Configuration;

namespace TodoApi.Service
{
    public class TodoDataService : ITodoDataService
    {
        private readonly ILogger<TodoDataService> _logger;
        private readonly string _connectionString;

        public TodoDataService(ILogger<TodoDataService> logger, IConfiguration configuration)
        {
            _logger = logger;
            _connectionString = configuration.GetConnectionString("TodoDb");
        }

        public async Task<IEnumerable<TodoItem>> GetAllAsync()
        {
            using var connection = new SQLiteConnection(_connectionString); // butina atlaisvint vieta (naudojantis using), nes .NET garbage collector neatlaisvina atminties tokiu variables kaip pvz.: file handles, db connections.
            var items = await connection.QueryAsync<TodoItem>("SELECT * FROM TodoItems"); 
            return items.ToList();
        }

        public async Task<TodoItem?> GetByIdAsync(long id)
        {
            using var connection = new SQLiteConnection(_connectionString);
            var item = await connection.QueryFirstOrDefaultAsync<TodoItem>("SELECT * FROM TodoItems WHERE Id = @Id", new { Id = id }); // 
            if (item == null)
                _logger.LogWarning($"Todo item with id: {id} not found.");
            return item;
        }
        
        public async Task<TodoItem> AddAsync(TodoItem item)
        {
            using var connection = new SQLiteConnection(_connectionString);
            var sql = "INSERT INTO TodoItems (Name, IsComplete) VALUES (@Name, @IsComplete); SELECT last_insert_rowid()";
            var id = await connection.QuerySingleAsync<long>(sql, item);
            item.Id = id;
            return item;
        }

        public async Task UpdateAsync(TodoItem newItem)
        {
            using var connection = new SQLiteConnection(_connectionString);
            var sql = "UPDATE TodoItems SET Name = @Name, IsComplete = @IsComplete WHERE Id = @Id";
            await connection.ExecuteAsync(sql, newItem);
            _logger.LogInformation($"Todo item with id: {newItem.Id} updated.");
        }

        public async Task DeleteAsync(long id)
        {
            using var connection = new SQLiteConnection(_connectionString);
            var sql = "DELETE FROM TodoItems WHERE Id = @Id";
            await connection.ExecuteAsync(sql, new { Id = id });
            _logger.LogInformation($"Todo item with id: {id} deleted.");
        }
    }
}