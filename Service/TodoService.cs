using TodoApi.Interfaces;
using TodoApi.Models;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using Microsoft.Extensions.Logging;

namespace TodoApi.Service
{ // await tik imituoja ilgai trunkancia operacija, labiau skirta ateiciai, jei butu norima prijungti DB
    public class TodoService : ITodoService                 // ITodoService interface implementacija
    {
        private readonly ILogger<TodoService> _logger;      // ILogger objektas, naudojamas loginti informacija
        private readonly List<TodoItem> _todoItems = new(); // List<TodoItem> objektas, kuriame saugomi visi TodoItem objektai
        private long _nextId = 1;                           // sekantis id, kuris bus priskiriamas naujiems TodoItem objektams

        public TodoService(ILogger<TodoService> logger) 
        {
            _logger = logger; // 
        }      

        public async Task<IEnumerable<TodoItem>> GetAllAsync() // grazina visus TodoItem objektus
        {
            _logger.LogInformation("Getting all todo items.");
            return await Task.FromResult(_todoItems);       // grazina _todoItems kaip IEnumerable<TodoItem>
        }

        public async Task<TodoItem?> GetByIdAsync(long id)  // grazina TodoItem objekta pagal id
        {
            _logger.LogInformation($"Getting todo item with id: {id}");
            var item = _todoItems.FirstOrDefault(item => item.Id == id); // paima pirma elementa, kurio id atitinka nurodyta id
            _logger.LogInformation(item == null ? $"Todo item with id: {id} not found." : $"Got todo item with id: {id}");
            return await Task.FromResult(item);             // grazina item kaip TodoItem? (nullable)
        }

        public async Task<TodoItem> AddAsync(TodoItem item) // prideda nauja TodoItem objekta
        {
            item.Id = _nextId++;                // priskiria naujam item id ir padidina _nextId
            _todoItems.Add(item);               // prideda nauja item i _todoItems
            _logger.LogInformation($"Added new todo item: {item.Name}, id: {item.Id}");
            return await Task.FromResult(item); // grazina nauja item
        }

        public async Task UpdateAsync(TodoItem newItem)        // atnaujina esama TodoItem objekta
        {
            var index = _todoItems.FindIndex(item => item.Id == newItem.Id); // paima indeksa, kurio id atitinka nurodyta id
            if (index != -1)   // Jei toks ID egzistuoja
            {             
                _todoItems[index] = newItem; // pakeicia esama item nauju item
                _logger.LogInformation($"Updated todo item with id: {newItem.Id}");
                await Task.CompletedTask; // task uzbaigtas
            } 
            else 
            {
                _logger.LogError($"Todo item with id: {newItem.Id} not found for update.");
            }
        }

        public async Task DeleteAsync(long id)
        {
            var index = _todoItems.FindIndex(item => item.Id == id); // paima indeksa, kurio id atitinka nurodyta id
            if (index != -1)    // Jei toks ID egzistuoja
            {                   
                _todoItems.RemoveAt(index);     // istrina item is _todoItems
                _logger.LogInformation($"Deleted todo item with id: {id}");
                await Task.CompletedTask;       // task uzbaigtas
            } 
            else
            {
                _logger.LogError($"Todo item with id: {id} not found for delete.");
            }
        }
    }
}