using System;
using System.IO;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;

namespace CDNMiddleware.Api
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var host = CreateHostBuilder(args).Build();

            //https://dotnetthoughts.net/seed-database-in-aspnet-core/
            //https://johanvergeer.github.io/posts/asp-net-core-seeding-databse
            host.Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .UseContentRoot(Directory.GetCurrentDirectory())
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.ConfigureKestrel(options =>
                    {
                        options.Limits.KeepAliveTimeout = TimeSpan.FromMinutes(15);
                        options.Limits.MaxRequestBodySize = 100 * 1024;
                    });
                    webBuilder.UseKestrel();
                    webBuilder.UseStartup<Startup>();
                });
    }
}