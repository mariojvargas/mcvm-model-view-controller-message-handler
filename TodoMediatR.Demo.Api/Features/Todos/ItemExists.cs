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
    public class ItemExists
    {
        public class Query : IRequest<bool>
        {
            public long Id { get; set; }
        }

        public class QueryHandler : IRequestHandler<Query, bool>
        {
            private readonly TodoDbContext _context;

            public QueryHandler(TodoDbContext context, IMapper mapper)
            {
                _context = context ?? throw new ArgumentNullException(nameof(context));
            }

            public async Task<bool> Handle(Query request, CancellationToken cancellationToken)
            {
                return await _context.TodoItem.AnyAsync(item => item.Id == request.Id);
            }
        }
    }
}
