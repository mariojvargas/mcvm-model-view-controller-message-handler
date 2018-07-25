using Microsoft.EntityFrameworkCore;

namespace TodoApiMediatR.Demo.Api.Infrastructure.Data
{
    public class TodoDbContext : DbContext
    {
        public TodoDbContext(DbContextOptions<TodoDbContext> options) : base(options)
        {
        }

        public DbSet<TodoItem> TodoItem { get; set; }
    }
}
