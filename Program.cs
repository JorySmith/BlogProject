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
            // Remove CreateHostBuilder Run command, store in host
            var host = CreateHostBuilder(args).Build();

            // Store registered DataService in dataService
            var dataService = host.Services
                                  .CreateScope()
                                  .ServiceProvider
                                  .GetRequiredService<DataService>();

            // Await call for ManageDataAsync
            await dataService.ManageDataAsync();

            // Run program
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
