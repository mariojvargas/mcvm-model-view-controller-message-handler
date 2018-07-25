using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace TodoApiMediatR.Demo.Api.Infrastructure.Data
{
    public static class TodosDbInitializer
    {
        public static void Initialize(TodoDbContext context)
        {
            // context.Database.EnsureCreated();

            if (context.TodoItem.Any())
            {
                return;
            }

            var todos = new [] {
                new TodoItem
                {
                    Name = "Attend and listen to my presentation",
                    IsComplete = true
                },
                new TodoItem
                {
                    Name = "Study this code",
                    IsComplete = false
                },
                new TodoItem
                {
                    Name = "Learn more about ASP.NET Core",
                    IsComplete = false
                },
                new TodoItem
                {
                    Name = "Learn more about MediatR",
                    IsComplete = false
                },
                new TodoItem
                {
                    Name = "Implement MVCM in your projects",
                    IsComplete = false
                },
            };

            context.TodoItem.AddRange(todos);
            context.SaveChanges();
        }
    }
}
