using System;
using System.ComponentModel.DataAnnotations;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.EntityFrameworkCore;
using TodoApiMediatR.Demo.Api.Infrastructure.Data;

namespace TodoApiMediatR.Demo.Api.Features.Todos
{
    public class UpdateItem
    {
        public class Command : IRequest<ConfirmedUpdatedTodoItemDto>
        {
            [Required]
            public long Id { get; set; }
            public UpdateTodoItemDto Dto { get; internal set; }
        }

        public class CommandHandler : IRequestHandler<Command, ConfirmedUpdatedTodoItemDto>
        {
            private readonly TodoDbContext _context;
            private readonly IMapper _mapper;
            private readonly IMediator _mediator;

            public CommandHandler(TodoDbContext context, IMapper mapper, IMediator mediator)
            {
                _context = context ?? throw new ArgumentNullException(nameof(context));
                _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
                _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
            }

            public async Task<ConfirmedUpdatedTodoItemDto> Handle(Command request, CancellationToken cancellationToken)
            {
                if (!await _mediator.Send(new ItemExists.Query { Id = request.Id }))
                {
                    return null;
                }

                var todoItemEntityToUpdate = _mapper.Map<TodoItem>(request);
                _context.Entry(todoItemEntityToUpdate).State = EntityState.Modified;
                await _context.SaveChangesAsync();

                return _mapper.Map<ConfirmedUpdatedTodoItemDto>(todoItemEntityToUpdate);
            }
        }
    }

    public class UpdateTodoItemDto
    {
        [Required]
        [StringLength(256)]
        public string Name { get; set; }

        public bool IsComplete { get; set; }

        public override string ToString() => $"{{Name = \"{Name}\"; IsComplete = {IsComplete}}}";
    }

    public class ConfirmedUpdatedTodoItemDto
    {
        public string Name { get; set; }

        public bool IsComplete { get; set; }
    }
}
