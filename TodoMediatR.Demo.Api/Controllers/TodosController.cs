using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.JsonPatch;
using TodoApiMediatR.Demo.Api.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using MediatR;
using TodoApiMediatR.Demo.Api.Features.Todos;

namespace TodoApiMediatR.Demo.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TodosController : ControllerBase
    {
        private readonly TodoDbContext _context;
        private IMediator _mediator;

        public TodosController(TodoDbContext context, IMediator mediator)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }

        [HttpGet]
        public async Task<IActionResult> GetAllTodos([FromQuery] ListAll.Query query)
        {
            var todoItems = await _mediator.Send(query);

            return Ok(todoItems);
        }

        [HttpGet("{id}", Name = "GetTodoById")]
        public async Task<IActionResult> GetById(long id)
        {
            var item = await _mediator.Send(new GetItemById.Query
            {
                Id = id
            });

            if (item == null)
            {
                return NotFound();
            }

            return Ok(item);
        }

        [HttpPost]
        public async Task<IActionResult> CreateItem([FromBody] CreateItem.Command command)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var item = await _mediator.Send(command);

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
