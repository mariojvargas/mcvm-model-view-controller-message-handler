using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using MediatR;
using TodoApiMediatR.Demo.Api.Infrastructure.Data;
using AutoMapper;
using AutoMapper.QueryableExtensions;

namespace TodoApiMediatR.Demo.Api.Features.Todos
{
    public class GetItemById
    {
        public class Query : IRequest<TodoItemDto>
        {
            public long Id { get; set; }

            // public override string ToString()
            // {
            //     return $"{{Id = {Id}}}";
            // }
        }

        public class QueryHandler : IRequestHandler<Query, TodoItemDto>
        {
            private readonly TodoDbContext _context;
            private IMapper _mapper;

            public QueryHandler(TodoDbContext context, IMapper mapper)
            {
                _context = context ?? throw new ArgumentNullException(nameof(context));
                _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            }

            public async Task<TodoItemDto> Handle(Query request, CancellationToken cancellationToken)
            {
                var entity = await _context.TodoItem.SingleOrDefaultAsync(item => item.Id == request.Id);

                return _mapper.Map<TodoItemDto>(entity);
            }
        }
    }

    public class TodoItemDto
    {
        public string Name { get; set; }
        public bool IsComplete { get; set; }
    }
}
