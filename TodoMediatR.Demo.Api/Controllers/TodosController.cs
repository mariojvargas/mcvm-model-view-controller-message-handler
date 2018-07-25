using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.JsonPatch;
using TodoApiMediatR.Demo.Api.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace TodoApiMediatR.Demo.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TodosController : ControllerBase
    {
        private readonly TodoDbContext _context;

        public TodosController(TodoDbContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        [HttpGet]
        public async Task<IActionResult> GetAllTodos()
        {
            var todoItems = await _context.TodoItem.ToListAsync();

            return Ok(todoItems);
        }

        [HttpGet("{id}", Name = "GetTodoById")]
        public async Task<IActionResult> GetById(long id)
        {
            var item = await _context.TodoItem.FindAsync(id);
            if (item == null)
            {
                return NotFound();
            }

            return Ok(item);
        }

        [HttpPost]
        public async Task<IActionResult> CreateItem([FromBody] TodoItem item)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _context.TodoItem.Add(item);
            await _context.SaveChangesAsync();

            return CreatedAtRoute("GetTodoById", new { id = item.Id }, item);
        }

        // If you've never implemented the HTTP PATCH method, be advised that it's a recommended practice.
        // See: https://dotnetcoretutorials.com/2017/11/29/json-patch-asp-net-core/
        //      http://jsonpatch.com/
        [HttpPatch("{id}")]
        public async Task<IActionResult> PatchItem(long id, [FromBody] JsonPatchDocument<TodoItem> itemPatchDocument)
        {
            if (itemPatchDocument == null)
            {
                return BadRequest();
            }

            var itemToUpdate = await _context.TodoItem.FindAsync(id);
            if (itemToUpdate == null)
            {
                return NotFound();
            }

            itemPatchDocument.ApplyTo(itemToUpdate, ModelState);
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            await _context.SaveChangesAsync();

            return Ok(itemToUpdate);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteItem(long id)
        {
            var itemToDelete = await _context.TodoItem.FindAsync(id);
            if (itemToDelete == null)
            {
                return NotFound();
            }

            _context.TodoItem.Remove(itemToDelete);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
