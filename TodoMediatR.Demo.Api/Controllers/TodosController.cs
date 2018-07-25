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

        // If you've never implemented the HTTP PATCH method, be advised that
        // it's a recommended practice for partial updates.
        // See: https://dotnetcoretutorials.com/2017/11/29/json-patch-asp-net-core/
        //      http://jsonpatch.com/
        //      https://stackoverflow.com/questions/24241893/rest-api-patch-or-put
        [HttpPatch("{id}")]
        public async Task<IActionResult> PatchItem(long id, [FromBody] JsonPatchDocument<UpdateTodoItemDto> itemPatchDocument)
        {
            if (itemPatchDocument == null)
            {
                return BadRequest();
            }

            var todoItemDto = await _mediator.Send(new GetItemById.Query
            {
                Id = id
            });

            if (todoItemDto == null)
            {
                return NotFound();
            }

            var itemToUpdateDto = new UpdateTodoItemDto
            {
                Name = todoItemDto.Name,
                IsComplete = todoItemDto.IsComplete
            };
            itemPatchDocument.ApplyTo(itemToUpdateDto, ModelState);

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var updatedItemDto = await _mediator.Send(new UpdateItem.Command
            {
                Id = id,
                Dto = itemToUpdateDto
            });

            if (updatedItemDto == null)
            {
                return NotFound();
            }

            return Ok(updatedItemDto);
        }

        // Here's an example using HTTP PUT in comparison to HTTP PATCH.
        // Notice we're still using the same UpdateItem.Command and UpdateTodoItemDto as
        // in the PATCH implementation above
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateItem(long id, [FromBody] UpdateTodoItemDto itemToUpdateDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var updatedItemDto = await _mediator.Send(new UpdateItem.Command
            {
                Id = id,
                Dto = itemToUpdateDto
            });

            if (updatedItemDto == null)
            {
                return NotFound();
            }

            return Ok(updatedItemDto);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteItem(long id)
        {
            var deletedItemDto = await _mediator.Send(new DeleteItem.Query
            {
                Id = id
            });

            if (deletedItemDto == null)
            {
                return NotFound();
            }

            return NoContent();
        }
    }
}
