using TodoApi.Interfaces;
using TodoApi.Models;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;

namespace TodoApi.Service
{ // await tik imituoja ilgai trunkancia operacija, labiau skirta ateiciai, jei butu norima prijungti DB
    public class TodoService : ITodoService
    {
        private readonly List<TodoItem> _todoItems = new();
        private long _nextId = 1;

        public async Task<IEnumerable<TodoItem>> GetAllAsync()
        {
            return await Task.FromResult(_todoItems);
        }

        public async Task<TodoItem?> GetByIdAsync(long id)
        {
            var item = _todoItems.FirstOrDefault(item => item.Id == id);
            return await Task.FromResult(item);
        }

        public async Task<TodoItem> AddAsync(TodoItem item)
        {
            item.Id = _nextId++;
            _todoItems.Add(item);
            return await Task.FromResult(item);
        }

        public async Task UpdateAsync(TodoItem item)
        {
            var index = _todoItems.FindIndex(item => item.Id == item.Id);
            if (index != -1){
                _todoItems[index] = item;
                await Task.CompletedTask;
            }
        }

        public async Task DeleteAsync(long id)
        {
            var index = _todoItems.FindIndex(item => item.Id == id);
            if (index != -1){
                _todoItems.RemoveAt(index);
                await Task.CompletedTask;
            }
        }
    }
}