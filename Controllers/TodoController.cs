using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using TodoApi.Interfaces;
using TodoApi.Models;
using Microsoft.Extensions.Logging; // skirtas logginimui

[Route("api/[controller]")] // nurodo, kad URL prasides /api/Todo
[ApiController]             // naudojant ApiController atributa, .NET Core automatiskai atlieka kai kuriuos veiksmus, pvz. model binding, validacija, HTTP status code nustatymas ir t.t.
public class TodoController : ControllerBase        // ControllerBase leidzia handlinti HTTP requests
{
    private readonly ITodoService _todoService;     // privatus laukas, kuriame saugomas ITodoService objektas
    private readonly ILogger<TodoController> _logger;

    public TodoController(ITodoService todoService, ILogger<TodoController> logger) // dependency injection - konstruktorius priima ITodoService objekta
    {
        _todoService = todoService;                 // priskiria konstruktoriuje gauta ITodoService objekta privaciam laukui
        _logger = logger;
    }

    // GET: api/Todo
    [HttpGet]
    public async Task<ActionResult<IEnumerable<TodoItem>>> GetAllAsync(CancellationToken cancellationToken = default) // ActionResult<T> grazina T arba status code
    {
        var items = await _todoService.GetAllAsync(cancellationToken); 
        return Ok(items); // Ok() grazina 200 status code ir items kaip response body
    }

    // GET: api/Todo/{id}
    [HttpGet("{id}", Name = "GetTodoItem")]   
    public async Task<ActionResult<TodoItem>> GetByIdAsync(long id, CancellationToken cancellationToken = default)
    {
        var item = await _todoService.GetByIdAsync(id, cancellationToken);
        if (item == null)
            return NotFound(); // NotFound() grazina 404 status code
        
        return Ok(item);       // Ok() grazina 200 status code ir item kaip response body
    }

    // POST: api/Todo
    [HttpPost]
    public async Task<ActionResult<TodoItem>> AddAsync(TodoItem item, CancellationToken cancellationToken = default)
    {
        _logger.LogInformation($"Adding new item: {item.Name}"); // pranesa, kai bandoma prideti nauja item
        var newItem = await _todoService.AddAsync(item, cancellationToken);
        if(newItem == null) 
        {
            _logger.LogError("Failed to add new item.");
            return Problem("Failed to create new item."); 
        }
        return CreatedAtRoute("GetTodoItem", new { id = newItem.Id }, newItem); // Nueina i GetTodoItem metoda ir grazina 201 status code ir newItem kaip response body
    }

    // PUT: api/Todo/{id}
    [HttpPut("{id}")]
    public async Task<ActionResult> UpdateAsync(long id, TodoItem item, CancellationToken cancellationToken = default)
    {
        if (id != item.Id)
            return BadRequest();              // BadRequest() grazina 400 status code
        
        await _todoService.UpdateAsync(item, cancellationToken); 
        return NoContent();                   // NoContent() grazina 204 status code
    }

    // DELETE: api/Todo/{id}
    [HttpDelete("{id}")]
    public async Task<ActionResult> DeleteAsync(long id, CancellationToken cancellationToken = default)
    {
        await _todoService.DeleteAsync(id, cancellationToken); 
        return NoContent();                 // NoContent() grazina 204 status code
    }    
}