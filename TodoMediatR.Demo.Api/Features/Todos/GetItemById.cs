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
        public class Query : IRequest<Result>
        {
            public long Id { get; set; }
        }

        public class Result
        {
            public string Name { get; set; }
            public bool IsComplete { get; set; }
        }

        public class QueryHandler : IRequestHandler<Query, Result>
        {
            private readonly TodoDbContext _context;
            private IMapper _mapper;

            public QueryHandler(TodoDbContext context, IMapper mapper)
            {
                _context = context ?? throw new ArgumentNullException(nameof(context));
                _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            }

            public async Task<Result> Handle(Query request, CancellationToken cancellationToken)
            {
                var entity = await _context.TodoItem.SingleOrDefaultAsync(item => item.Id == request.Id);
                if (entity ==  null) {
                    return null;
                }

                // See: https://stackoverflow.com/questions/36856073/the-instance-of-entity-type-cannot-be-tracked-because-another-instance-of-this-t
                _context.Entry(entity).State = EntityState.Detached;

                return _mapper.Map<Result>(entity);
            }
        }
    }
}
