using System;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Reports.Auth.Core.Security.Hashing;
using Reports.Auth.Data;

namespace Reports.Auth
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var host = CreateHostBuilder(args).Build();
            using(var scope = host.Services.CreateScope())
            {
                var serviceProvider = scope.ServiceProvider;
                try
                {
                    var services = scope.ServiceProvider;
                    var context = serviceProvider.GetRequiredService<AuthDbContext>();
                    DbSeed.Seed(context);
                    DbInitializer.Initialize(context);
                }
                catch (Exception exception)
                {
                }
            }

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