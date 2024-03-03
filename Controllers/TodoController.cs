using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using TodoApi.Interfaces;
using TodoApi.Models;

[Route("api/[controller]")] // nurodo, kad URL prasides /api/Todo
[ApiController]             // naudojant ApiController atributa, .NET Core automatiskai atlieka kai kuriuos veiksmus, pvz. model binding, validacija, HTTP status code nustatymas ir t.t.
public class TodoController : ControllerBase        // ControllerBase leidzia handlinti HTTP requests
{
    private readonly ITodoService _todoService;     // privatus laukas, kuriame saugomas ITodoService objektas
    
    public TodoController(ITodoService todoService) // dependency injection - konstruktorius priima ITodoService objekta
    {
        _todoService = todoService;                 // priskiria konstruktoriuje gauta ITodoService objekta privaciam laukui
    }

    // GET: api/Todo
    [HttpGet]
    public async Task<ActionResult<IEnumerable<TodoItem>>> GetAllAsync() // ActionResult<T> grazina T arba status code
    {
        var items = await _todoService.GetAllAsync(); 
        return Ok(items); // Ok() grazina 200 status code ir items kaip response body
    }

    // GET: api/Todo/{id}
    [HttpGet("{id}")]   
    public async Task<ActionResult<TodoItem>> GetByIdAsync(long id)
    {
        var item = await _todoService.GetByIdAsync(id);
        if (item == null)
            return NotFound(); // NotFound() grazina 404 status code
        
        return Ok(item);       // Ok() grazina 200 status code ir item kaip response body
    }

    // POST: api/Todo
    [HttpPost]
    public async Task<ActionResult<TodoItem>> AddAsync(TodoItem item)
    {
        var newItem = await _todoService.AddAsync(item); // newItem grazina sukurta irasa su id
        return CreatedAtAction(nameof(GetByIdAsync),     // CreatedAtAction() grazina 201 status code ir item kaip response body
        new { id = newItem.Id }, newItem);               // ir nurodo URL, kuriame galima gauti naujai sukurta irasa
    }

    // PUT: api/Todo/{id}
    [HttpPut("{id}")]
    public async Task<ActionResult> UpdateAsync(long id, TodoItem item)
    {
        if (id != item.Id)
            return BadRequest();              // BadRequest() grazina 400 status code
        
        await _todoService.UpdateAsync(item); 
        return NoContent();                   // NoContent() grazina 204 status code
    }

    // DELETE: api/Todo/{id}
    [HttpDelete("{id}")]
    public async Task<ActionResult> DeleteAsync(long id)
    {
        await _todoService.DeleteAsync(id); 
        return NoContent();                 // NoContent() grazina 204 status code
    }    
}