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
        public class Command : IRequest<Result>
        {
            [Required]
            [StringLength(256)]
            public string Name { get; set; }

            public override string ToString() => $"{{Name = \"{Name}\"}}";
        }

        public class Result
        {
            public long Id { get; set; }
            public string Name { get; set; }
            public bool IsComplete { get; set; }
        }

        public class CommandHandler : IRequestHandler<Command, Result>
        {
            private readonly TodoDbContext _context;
            private readonly IMapper _mapper;

            public CommandHandler(TodoDbContext context, IMapper mapper)
            {
                _context = context ?? throw new ArgumentNullException(nameof(context));
                _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            }

            public async Task<Result> Handle(Command request, CancellationToken cancellationToken)
            {
                var itemToAdd = _mapper.Map<TodoItem>(request);

                _context.TodoItem.Add(itemToAdd);
                await _context.SaveChangesAsync();

                return _mapper.Map<Result>(itemToAdd);
            }
        }
    }
}
