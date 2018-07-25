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
    public class ListAll
    {
        public class Query : IRequest<IEnumerable<TodoListItemDto>>
        {
            public override string ToString() => "{}";
        }

        public class QueryHandler : IRequestHandler<Query, IEnumerable<TodoListItemDto>>
        {
            private readonly TodoDbContext _context;
            private IConfigurationProvider _mappingConfigurationProvider;

            public QueryHandler(TodoDbContext context, IConfigurationProvider mappingConfigurationProvider)
            {
                _context = context ?? throw new ArgumentNullException(nameof(context));
                _mappingConfigurationProvider = mappingConfigurationProvider ?? throw new ArgumentNullException(nameof(mappingConfigurationProvider));
            }

            public async Task<IEnumerable<TodoListItemDto>> Handle(Query request, CancellationToken cancellationToken)
            {
                return await _context.TodoItem.ProjectTo<TodoListItemDto>(_mappingConfigurationProvider).ToListAsync();
            }
        }
    }

    public class TodoListItemDto
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public bool IsComplete { get; set; }
    }
}
