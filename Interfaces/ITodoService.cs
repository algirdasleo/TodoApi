using TodoApi.Models;

namespace TodoApi.Interfaces
{
    public interface ITodoService
    {
        Task<IEnumerable<TodoItem>> GetAllAsync(CancellationToken cancellationToken = default); // IEnumerable paslepia return data structure ir patogu naudot su LINQ (pvz filtravimui, rikiavimui, grupavimui ir t.t.)
        Task<TodoItem?> GetByIdAsync(long id, CancellationToken cancellationToken = default);     // grazina null, jei nera tokio id
        Task<TodoItem> AddAsync(TodoItem item, CancellationToken cancellationToken = default);    // grazina sukurta irasa su id
        Task UpdateAsync(TodoItem item, CancellationToken cancellationToken = default);   
        Task DeleteAsync(long id, CancellationToken cancellationToken = default);
    }
}