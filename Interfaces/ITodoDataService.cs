using TodoApi.Models;

namespace TodoApi.Interfaces
{
    public interface ITodoDataService
    {
        Task<IEnumerable<TodoItem>> GetAllAsync(CancellationToken cancellationToken = default);
        Task<TodoItem?> GetByIdAsync(long id, CancellationToken cancellationToken = default);
        Task<TodoItem> AddAsync(TodoItem item, CancellationToken cancellationToken = default);
        Task UpdateAsync(TodoItem newItem, CancellationToken cancellationToken = default);
        Task DeleteAsync(long id, CancellationToken cancellationToken = default);
    }
}