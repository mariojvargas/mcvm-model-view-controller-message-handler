using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using TodoApiMediatR.Demo.Api.Infrastructure.Data;

namespace TodoApiMediatR.Demo.Api
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var host = CreateWebHostBuilder(args).Build();

            using (var scope = host.Services.CreateScope())
            {
                var services = scope.ServiceProvider;

                InitializeDatabase(services);
            }

            host.Run();
        }

        public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .UseStartup<Startup>();

        private static void InitializeDatabase(IServiceProvider services)
        {
            var logger = services.GetRequiredService<ILogger<Program>>();

            try
            {
                logger.LogInformation("Initializing database");

                var context = services.GetRequiredService<TodoDbContext>();
                TodosDbInitializer.Initialize(context);

                logger.LogInformation("Database initialized");
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "An error occurred while initializing the database");
            }
        }
    }
}
