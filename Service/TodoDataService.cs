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
using TodoApi.Helpers;

namespace TodoApi.Service
{
    public class TodoDataService : ITodoDataService
    {
        private readonly ILogger<TodoDataService> _logger;
        private readonly SQLConnectionFactory _connectionFactory;
        public TodoDataService(ILogger<TodoDataService> logger, SQLConnectionFactory connectionFactory)
        {
            _logger = logger;
            _connectionFactory = connectionFactory;
        }

        public async Task<IEnumerable<TodoItem>> GetAllAsync(CancellationToken cancellationToken)
        {
            using var connection = await _connectionFactory.CreateConnectionAsync();
            var command = new CommandDefinition("SELECT * FROM TodoItems", cancellationToken: cancellationToken); // CommandDefinition objektas, naudojamas nurodyti uzklausos parametrus
            var items = await connection.QueryAsync<TodoItem>(command); 
            return items.ToList();
        }

        public async Task<TodoItem?> GetByIdAsync(long id, CancellationToken cancellationToken)
        {
            using var connection = await _connectionFactory.CreateConnectionAsync();
            var command = new CommandDefinition("SELECT * FROM TodoItems WHERE Id = @Id", new { Id = id }, cancellationToken: cancellationToken);
            var item = await connection.QueryFirstOrDefaultAsync<TodoItem>(command);
            if (item == null)
                _logger.LogWarning($"Todo item with id: {id} not found.");
            return item;
        }
        
        public async Task<TodoItem> AddAsync(TodoItem item, CancellationToken cancellationToken)
        {
            using var connection = await _connectionFactory.CreateConnectionAsync();
            var command = new CommandDefinition("INSERT INTO TodoItems (Name, IsComplete) VALUES (@Name, @IsComplete); SELECT last_insert_rowid()", item, cancellationToken: cancellationToken);
            var id = await connection.QuerySingleAsync<long>(command);
            item.Id = id;
            return item;
        }

        public async Task UpdateAsync(TodoItem newItem, CancellationToken cancellationToken)
        {
            using var connection = await _connectionFactory.CreateConnectionAsync();
            var command = new CommandDefinition("UPDATE TodoItems SET Name = @Name, IsComplete = @IsComplete WHERE Id = @Id", newItem, cancellationToken: cancellationToken);
            await connection.ExecuteAsync(command);
            _logger.LogInformation($"Todo item with id: {newItem.Id} updated.");
        }

        public async Task DeleteAsync(long id, CancellationToken cancellationToken)
        {
            using var connection = await _connectionFactory.CreateConnectionAsync();
            var command = new CommandDefinition("DELETE FROM TodoItems WHERE Id = @Id", new { Id = id }, cancellationToken: cancellationToken);
            await connection.ExecuteAsync(command);
            _logger.LogInformation($"Todo item with id: {id} deleted.");
        }
    }
}