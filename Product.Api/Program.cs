using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Product.Api.Data;

namespace Product.Api
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var host = CreateHostBuilder(args).Build();
            await RunDatabaseMigrationsAsync(host);
            await host.RunAsync();
        }

        private static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });

        private static async Task RunDatabaseMigrationsAsync(IHost host)
        {
            using var serviceScope = host.Services.CreateScope();
            var provider = serviceScope.ServiceProvider;
            var dbContext = provider.GetRequiredService<DataContext>();
            await dbContext.Database.MigrateAsync();
        }
    }
}
