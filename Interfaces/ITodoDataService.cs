using TodoApi.Models;
namespace TodoApi.Interfaces
{
    public interface ITodoDataService
    {
        Task<IEnumerable<TodoItem>> GetAllAsync();
        Task<TodoItem?> GetByIdAsync(long id);
        Task<TodoItem> AddAsync(TodoItem item);
        Task UpdateAsync(TodoItem newItem);
        Task DeleteAsync(long id);
    }
}