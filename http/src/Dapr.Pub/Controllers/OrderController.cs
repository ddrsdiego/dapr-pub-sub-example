namespace Dapr.Pub.Controllers
{
    using System.Net.Http;
    using System.Net.Http.Headers;
    using System.Text;
    using System.Text.Json;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Mvc;
    using Models;
    using Requests;

    internal static class Topics
    {
        public const string OrderRedeemCreated = "order-redeem-created";
        public const string OrderInvestmentCreated = "order-investment-created";
    }

    internal static class PubSubNames
    {
        public const string InvestmentsOrders = "investments-orders";
    }

    [ApiController]
    [Route("orders")]
    public class OrderController : ControllerBase
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public OrderController(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        [HttpPost("invest")]
        public async Task<IActionResult> RequestNewInvestmentOrder([FromBody] NewOrderInvestmentRequest request)
        {
            var newOrder = new Order(request.Quantity, request.UnitPrice);

            await SaveStateAsync("order-investment", newOrder);
            _ = PublishOrderInvestmentCreated(newOrder);

            return Accepted(newOrder);
        }

        [HttpPost("redeem")]
        public async Task<IActionResult> RequestNewRedeemOrder([FromBody] NewOrderRedeemRequest request)
        {
            var newOrder = new Order(request.Quantity, request.UnitPrice);

            await SaveStateAsync("order-redeem", newOrder);
            _ = PublishOrderRedeemCreated(newOrder);

            return Accepted(newOrder);
        }

        private async Task PublishOrderInvestmentCreated(Order newOrder)
        {
            var httpClient = _httpClientFactory.CreateClient(Topics.OrderInvestmentCreated);
            httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            var orderJson = JsonSerializer.Serialize(newOrder);

            var content = new StringContent(orderJson, Encoding.UTF8, "application/json");
            var httpResponseTask =
                await httpClient.PostAsync(
                    $"http://localhost:3501/v1.0/publish/{PubSubNames.InvestmentsOrders}/{Topics.OrderInvestmentCreated}",
                    content);
        }

        private async Task PublishOrderRedeemCreated(Order newOrder)
        {
            var httpClient = _httpClientFactory.CreateClient(Topics.OrderRedeemCreated);
            httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            var orderJson = JsonSerializer.Serialize(newOrder);

            var content = new StringContent(orderJson, Encoding.UTF8, "application/json");
            var httpResponseTask =
                await httpClient.PostAsync(
                    $"http://localhost:3501/v1.0/publish/{PubSubNames.InvestmentsOrders}/{Topics.OrderRedeemCreated}",
                    content);
        }

        private async Task SaveStateAsync(string stateName, Order newOrder)
        {
            var httpClient = _httpClientFactory.CreateClient(stateName);
            httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            var key = $"{stateName.ToLowerInvariant()}-{newOrder.OrderId}";
            var orderAsJson = JsonSerializer.Serialize(
                new[]
                {
                    new
                    {
                        key = key,
                        value = newOrder
                    }
                }
            );

            var state = new StringContent(orderAsJson, Encoding.UTF8, "application/json");
            var stateResponseMessage =
                await httpClient.PostAsync("http://localhost:3501/v1.0/state/order-investment", state);
        }
    }
}