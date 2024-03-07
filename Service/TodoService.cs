using TodoApi.Interfaces;
using TodoApi.Models;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using Microsoft.Extensions.Logging;

namespace TodoApi.Service
{
    public class TodoService : ITodoService                 // ITodoService interface implementacija
    {
        private readonly ILogger<TodoService> _logger;      // ILogger objektas, naudojamas loginti informacija
        private readonly ITodoDataService _todoDataService; // ITodoDataService objektas, naudojamas gauti duomenis is duomenu bazes

        public TodoService(ILogger<TodoService> logger, ITodoDataService todoDataService) // dependency injection - per konstruktoriu perduodami reikalingi objektai
        {
            _logger = logger;
            _todoDataService = todoDataService;
        }

        public async Task<IEnumerable<TodoItem>> GetAllAsync() // grazina visus TodoItem objektus
        {
            _logger.LogInformation("Getting all todo items.");
            return await _todoDataService.GetAllAsync();      
        }

        public async Task<TodoItem?> GetByIdAsync(long id)    // grazina TodoItem objekta pagal id
        {
            _logger.LogInformation($"Getting todo item with id: {id}");
            return await _todoDataService.GetByIdAsync(id);
        }

        public async Task<TodoItem> AddAsync(TodoItem item)   // prideda nauja TodoItem objekta
        {
            _logger.LogInformation("Adding new todo item.");
            return await _todoDataService.AddAsync(item);
        }

        public async Task UpdateAsync(TodoItem newItem)       // atnaujina esama TodoItem objekta
        {
            _logger.LogInformation($"Updating todo item with id: {newItem.Id}");
            await _todoDataService.UpdateAsync(newItem);
        }

        public async Task DeleteAsync(long id)               // istrina TodoItem objekta pagal id
        {
            _logger.LogInformation($"Deleting todo item with id: {id}");
            await _todoDataService.DeleteAsync(id);
        }
    }
}