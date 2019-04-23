using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TodoApi.Models;
using Microsoft.AspNetCore.JsonPatch;

namespace TodoApi.Controllers
{
    [Route("api/todo")]
    [ApiController]
    public class TodoController : ControllerBase
    {
        private readonly TodoContext _context;

        public TodoController(TodoContext context)
        {
           _context = context;

           if (_context.Todos.Count() == 0)
           {
               _context.Todos.Add(
                   new Todo { Name = "Item 1"}
               );
               _context.SaveChanges();
           } 
        }

        #region HTTP GET Todo
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Todo>>> GetTodos()
        {
            return await _context.Todos.ToListAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Todo>> GetTodo(int id)
        {
            var todo = await _context.Todos.FindAsync(id);

            if (todo == null)
            {
                return NotFound();
            }

            return todo;
        }
        #endregion

        #region HTTP POST Create Todo
        [HttpPost]
        public async Task<ActionResult<Todo>> PostTodo(Todo todo)
        {
            _context.Todos.Add(todo);
            await _context.SaveChangesAsync();

            return CreatedAtAction(
                nameof(GetTodo), new { id = todo.Id }, todo
            );
        }
        #endregion

        #region HTTP PUT Todo
        [HttpPut("{id}")]
        public async Task<IActionResult> PutTodo(int id, Todo todo)
        {
            if (id == todo.Id)
            {
                return BadRequest();
            }

            _context.Entry(todo).State = EntityState.Modified;

            await _context.SaveChangesAsync();
            
            return NoContent();
        }
        #endregion

        #region HTTP Patch Todo
        // Todo: Finish patch implementation
        [HttpPatch("{id}")]

        public async Task<ActionResult<Todo>> PatchTodo(int id, [FromBody]JsonPatchDocument<Todo> todoPatch)
        {
            var todo = await _context.Todos.FindAsync(id);
            
            todoPatch.ApplyTo(todo);

            _context.Update(todo);

            return todo;
        }
        #endregion

        #region HTTP DELETE Todo
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTodo(int id)
        {
            var todo = await _context.Todos.FindAsync(id);

            if (todo == null)
            {
                return NotFound();
            }

            _context.Todos.Remove(todo);
            await _context.SaveChangesAsync();

            return NoContent();
        }
        #endregion
    }
}