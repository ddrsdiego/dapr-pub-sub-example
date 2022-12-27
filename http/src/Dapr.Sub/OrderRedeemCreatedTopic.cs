namespace Dapr.Sub
{
    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Routing;
    using Microsoft.Extensions.DependencyInjection;
    using Models;
    using Services;

    internal static class OrderRedeemCreatedTopic
    {
        public static void MapOrderRedeemCreatedTopic(this IEndpointRouteBuilder endpoints)
        {
            endpoints.MapPost("order-redeem-created", async context =>
            {
                var body = await context.Request.ReadFromJsonAsync<DaprData<Order>>();
                var orderProcessor = context.RequestServices.GetRequiredService<IOrderProcessor>();

                await orderProcessor.Process(body.Data);
            });
        }
    }
}