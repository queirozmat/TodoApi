using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TodoApi.Data;
using TodoApi.Models;

[Route("api/tarefas")]
[ApiController]
public class TodoController : ControllerBase
{
    private readonly AppDbContext _context;

    public TodoController(AppDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<TodoItem>>> GetTodoItems()
    {
        var todoItems = await _context.TodoItems.ToListAsync();
        return todoItems;
    }


   
    [HttpPost]
    public async Task<ActionResult<TodoItem>> PostTodoItem(TodoItem todo)
    {
    if (todo.Id == 0) 
    {
        int maxId = await _context.TodoItems.MaxAsync(t => (int?)t.Id) ?? 0;
        
        todo.Id = maxId + 1;
    }

    _context.TodoItems.Add(todo);
     await _context.SaveChangesAsync();

    return CreatedAtAction(nameof(GetTodoItems), new { id = todo.Id }, todo);

    }   

    [HttpPut]
    public async Task<IActionResult> UpdateTodoItem(TodoItem updatedTodoItem)
    {
    if (!ModelState.IsValid)
    {
        return BadRequest(ModelState);
    }

    try
    {
        _context.Entry(updatedTodoItem).State = EntityState.Modified;
        await _context.SaveChangesAsync();
    }
    catch (DbUpdateConcurrencyException)
    {
        if (!TodoItemExists(updatedTodoItem.Id))
        {
            return NotFound("O item não foi encontrado.");
        }
        else
        {
            throw;
        }
    }

    return Ok(updatedTodoItem);
    }

    [HttpDelete]
    public async Task<IActionResult> DeleteTodoItem([FromQuery] long id)
    {
    var todoItem = await _context.TodoItems.FindAsync(id);

    if (todoItem == null)
    {
        return NotFound();
    }

    _context.TodoItems.Remove(todoItem);
    await _context.SaveChangesAsync();

    return NoContent(); 
    }


    private bool TodoItemExists(long id)
    {
    return _context.TodoItems.Any(e => e.Id == id);
    }
}
