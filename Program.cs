using BlogProject.Services;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BlogProject
{
    public class Program
    {
        // Update to async Task program, not void
        public static async Task Main(string[] args)
        {
            // Run DataService ManageDataAsync() every time application
            // starts to migrate DB and see if roles and users need to be seeded
            // Remove Run command, store CreateHostBuilder in var host
            var host = CreateHostBuilder(args).Build();

            // Pull out registered DataServce, store in var dataService
            var dataService = host.Services
                                  .CreateScope()
                                  .ServiceProvider
                                  .GetRequiredService<DataService>();

            // Await call for ManageDataAsync
            await dataService.ManageDataAsync();

            // Run host
            host.Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }
}
