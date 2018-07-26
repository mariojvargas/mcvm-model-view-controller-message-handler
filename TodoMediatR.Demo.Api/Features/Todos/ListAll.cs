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
        public class Query : IRequest<IEnumerable<Result>>
        {
            public override string ToString() => "{}";
        }

        public class Result
        {
            public long Id { get; set; }
            public string Name { get; set; }
            public bool IsComplete { get; set; }
        }

        public class QueryHandler : IRequestHandler<Query, IEnumerable<Result>>
        {
            private readonly TodoDbContext _context;
            private IConfigurationProvider _mappingConfigurationProvider;

            public QueryHandler(TodoDbContext context, IConfigurationProvider mappingConfigurationProvider)
            {
                _context = context ?? throw new ArgumentNullException(nameof(context));
                _mappingConfigurationProvider = mappingConfigurationProvider ?? throw new ArgumentNullException(nameof(mappingConfigurationProvider));
            }

            public async Task<IEnumerable<Result>> Handle(Query request, CancellationToken cancellationToken)
            {
                return await _context.TodoItem.ProjectTo<Result>(_mappingConfigurationProvider).ToListAsync();
            }
        }
    }
}
