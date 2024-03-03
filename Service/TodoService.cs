using TodoApi.Interfaces;
using TodoApi.Models;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;

namespace TodoApi.Service
{ // await tik imituoja ilgai trunkancia operacija, labiau skirta ateiciai, jei butu norima prijungti DB
    public class TodoService : ITodoService                 // ITodoService interface implementacija
    {
        private readonly List<TodoItem> _todoItems = new(); // privatus laukas, kuriame saugomi visi TodoItem objektai
        private long _nextId = 1;                           // privatus laukas, kuris saugo sekancia id reiksme   

        public async Task<IEnumerable<TodoItem>> GetAllAsync() // grazina visus TodoItem objektus
        {
            return await Task.FromResult(_todoItems);       // grazina _todoItems kaip IEnumerable<TodoItem>
        }

        public async Task<TodoItem?> GetByIdAsync(long id)  // grazina TodoItem objekta pagal id
        {
            var item = _todoItems.FirstOrDefault(item => item.Id == id); // paima pirma elementa, kurio id atitinka nurodyta id
            return await Task.FromResult(item);             // grazina item kaip TodoItem? (nullable)
        }

        public async Task<TodoItem> AddAsync(TodoItem item) // prideda nauja TodoItem objekta
        {
            item.Id = _nextId++;                // priskiria naujam item id ir padidina _nextId
            _todoItems.Add(item);               // prideda nauja item i _todoItems
            return await Task.FromResult(item); // grazina nauja item
        }

        public async Task UpdateAsync(TodoItem item)        // atnaujina esama TodoItem objekta
        {
            var index = _todoItems.FindIndex(item => item.Id == item.Id); // paima indeksa, kurio id atitinka nurodyta id
            if (index != -1){             // Jei toks ID egzistuoja
                _todoItems[index] = item; // pakeicia esama item nauju item
                await Task.CompletedTask; // task uzbaigtas
            }
        }

        public async Task DeleteAsync(long id)
        {
            var index = _todoItems.FindIndex(item => item.Id == id); // paima indeksa, kurio id atitinka nurodyta id
            if (index != -1){                   // Jei toks ID egzistuoja
                _todoItems.RemoveAt(index);     // istrina item is _todoItems
                await Task.CompletedTask;       // task uzbaigtas
            }
        }
    }
}