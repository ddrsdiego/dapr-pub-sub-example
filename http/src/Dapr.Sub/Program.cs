namespace Dapr.Sub
{
    using System.Reflection;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.Extensions.Hosting;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.OpenApi.Models;
    using Services;
    using System.Threading.Tasks;

    public abstract class Program
    {
        public static async Task Main(string[] args)
        {
            await CreateHostBuilder(args)
                .Build()
                .RunAsync();
        }

        private static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                })
                .ConfigureServices((context, services) =>
                {
                    services.AddControllers();
                    services.AddSingleton<IOrderProcessor, OrderProcessor>();
                    services.AddSwaggerGen(c =>
                        c.SwaggerDoc("v1", new OpenApiInfo {Title = Assembly.GetEntryAssembly()?.GetName().Name}));
                });
    }
}