namespace Dapr.Sub
{
    using Models;
    using Services;
    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Routing;
    using Microsoft.Extensions.DependencyInjection;

    internal static class OrderInvestmentCreatedTopic
    {
        public static void MapOrderInvestmentCreatedTopic(this IEndpointRouteBuilder endpoints)
        {
            endpoints.MapPost("order-investment-created", async context =>
            {
                var body = await context.Request.ReadFromJsonAsync<DaprData<Order>>();
                
                var orderProcessor =
                    context.RequestServices.GetRequiredService<IOrderProcessor>();

                await orderProcessor.Process(body.Data);
            });
        }
    }
}