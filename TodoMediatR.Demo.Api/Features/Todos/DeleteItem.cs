using System;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using TodoApiMediatR.Demo.Api.Infrastructure.Data;

namespace TodoApiMediatR.Demo.Api.Features.Todos
{
    public class DeleteItem
    {
        public class Query : IRequest<Result>
        {
            public long Id { get; set; }

            public override string ToString()
            {
                return $"{{Id = {Id}}}";
            }
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
                if (entity == null)
                {
                    return null;
                }

                _context.TodoItem.Remove(entity);
                await _context.SaveChangesAsync();

                return _mapper.Map<Result>(entity);
            }
        }
    }
}
