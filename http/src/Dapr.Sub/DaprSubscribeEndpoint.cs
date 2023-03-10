namespace Dapr.Sub
{
    using System.Text.Json;
    using System.Text.Json.Serialization;
    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Routing;

    internal record DaprSubscription(
        [property: JsonPropertyName("pubsubname")]
        string PubsubName,
        [property: JsonPropertyName("topic")] string Topic,
        [property: JsonPropertyName("route")] string Route);
    
    public static class DaprSubscribeEndpoint
    {
        public static void MapDaprSubscriber(this IEndpointRouteBuilder endpoints)
        {
            endpoints.MapGet("/dapr/subscribe", async (context) =>
            {
                var orderInvestmentCreatedSub = new DaprSubscription(PubsubName: "investments-orders",
                    Topic: "order-investment-created",
                    Route: "order-investment-created");

                var orderRedeemCreatedSub = new DaprSubscription(PubsubName: "investments-orders",
                    Topic: "order-redeem-created",
                    Route: "order-redeem-created");

                await context.Response.WriteAsync(JsonSerializer.Serialize(
                    new[]
                    {
                        orderInvestmentCreatedSub, 
                        orderRedeemCreatedSub
                    },
                    new JsonSerializerOptions
                    {
                        PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                        DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull
                    }));
            });
        }
    }
}