using System;
using System.ComponentModel.DataAnnotations;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using TodoApiMediatR.Demo.Api.Infrastructure.Data;

namespace TodoApiMediatR.Demo.Api.Features.Todos
{
    public class CreateItem
    {
        public class Command : IRequest<CreatedTodoItemDto>
        {
            [Required]
            [StringLength(256)]
            public string Name { get; set; }

            public override string ToString() => $"{{Name = \"{Name}\"}}";
        }

        public class CommandHandler : IRequestHandler<Command, CreatedTodoItemDto>
        {
            private readonly TodoDbContext _context;
            private IMapper _mapper;

            public CommandHandler(TodoDbContext context, IMapper mapper)
            {
                _context = context ?? throw new ArgumentNullException(nameof(context));
                _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            }

            public async Task<CreatedTodoItemDto> Handle(Command request, CancellationToken cancellationToken)
            {
                var itemToAdd = _mapper.Map<TodoItem>(request);

                _context.TodoItem.Add(itemToAdd);
                await _context.SaveChangesAsync();

                return _mapper.Map<CreatedTodoItemDto>(itemToAdd);
            }
        }
    }

    public class CreatedTodoItemDto
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public bool IsComplete { get; set; }
    }
}
