using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TodoApp.Models;
using TodoApp.Services;

namespace TodoApp.Controllers
{
    [Route("api/TodoItems")]
    [ApiController]
    public class TodoItemsController : ControllerBase
    {
        private readonly TodoContext _context;
        private ActionResult<TodoItem> todoItem;
        private ImplementedMethods implementedMethods = new ImplementedMethods();

        public TodoItemsController(TodoContext context)
        {
            _context = context;
        }

        // GET: api/TodoItems
        [HttpGet]
        public async Task<ActionResult<IEnumerable<TodoItem>>> GetTodoItems()
        {
            return await _context.TodoItems.ToListAsync();
        }

        // GET: api/TodoItems/5
        [HttpGet("{start}")]
        public async Task<ActionResult<TodoItem>> GetTodoItem(int start)
        {
            if(start < 0)
            {
                return NotFound();
            }
            if (_context.TodoItems.Count() < start)
            {
                return NotFound();
            }

            return Ok(implementedMethods.GetNonOrderedObjectsByReference(start, _context));  // ConcurrentBag is orderless. I used it because it was suggested in the email. 
            //return Ok(implementedMethods.GetOrderedObjectsByReference(start, _context));  // To swicth an ordered solution, please  uncommend line 45 and then comment line 44 out.
        }



        // POST: api/TodoItems
        [HttpPost]
        public async Task<ActionResult<TodoItem>> PostTodoItem(TodoItem todoItem)
        {
            _context.TodoItems.Add(todoItem);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetTodoItem), new { start = todoItem.Id }, todoItem);
        }



        private bool TodoItemExists(long id)
        {
            return _context.TodoItems.Any(e => e.Id == id);
        }
    }
}
