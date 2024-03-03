using TodoApi.Models;

namespace TodoApi.Interfaces
{
    public interface ITodoService
    {
        Task<IEnumerable<TodoItem>> GetAllAsync(); // IEnumerable paslepia return data structure ir patogu naudot su LINQ (pvz filtravimui, rikiavimui, grupavimui ir t.t.)
        Task<TodoItem?> GetByIdAsync(long id);     // grazina null, jei nera tokio id
        Task<TodoItem> AddAsync(TodoItem item);    // grazina sukurta irasa su id
        Task UpdateAsync(TodoItem item);   
        Task DeleteAsync(long id);
    }
}