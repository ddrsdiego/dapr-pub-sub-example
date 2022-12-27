namespace Dapr.Sub
{
    using System.Text.Json;
    using System.Text.Json.Serialization;
    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Routing;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Hosting;
    using Services;

    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseRouting();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                endpoints.MapGetDaprSubscriber();
                endpoints.MapOrderRedeemCreatedTopic();
                endpoints.MapOrderInvestmentCreatedTopic();
            });
        }

        
    }

    public record DaprSubscription(
        [property: JsonPropertyName("pubsubname")]
        string PubsubName,
        [property: JsonPropertyName("topic")] string Topic,
        [property: JsonPropertyName("route")] string Route);

    public record DaprData<T>([property: JsonPropertyName("data")] T Data);
}