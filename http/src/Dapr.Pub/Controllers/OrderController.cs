namespace Dapr.Pub.Controllers
{
    using System;
    using System.Net.Http;
    using System.Net.Http.Headers;
    using System.Text;
    using System.Text.Json;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Mvc;
    using Models;
    using Requests;

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
        public async Task<IActionResult> RequestNewInvestmentOrder([FromBody] NewInvestmentOrderRequest newInvestmentOrderRequest)
        {
            var newOrder = new Order(Guid.NewGuid().ToString(), newInvestmentOrderRequest.Quantity, newInvestmentOrderRequest.UnitPrice);

            await SaveStateAsync(newOrder);
            await PublishOrderInvestmentCreated(newOrder);

            return Accepted(newOrder);
        }

        [HttpPost("redeem")]
        public async Task<IActionResult> RequestNewRedeemOrder([FromBody] NewInvestmentOrderRequest newInvestmentOrderRequest)
        {
            var newOrder = new Order(Guid.NewGuid().ToString(), newInvestmentOrderRequest.Quantity, newInvestmentOrderRequest.UnitPrice);

            await SaveStateAsync(newOrder);
            await PublishOrderRedeemCreated(newOrder);

            return Accepted(newOrder);
        }
        
        private async Task PublishOrderInvestmentCreated(Order newOrder)
        {
            var httpClient = _httpClientFactory.CreateClient("order");
            httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            var orderJson = JsonSerializer.Serialize(newOrder);

            var content = new StringContent(orderJson, Encoding.UTF8, "application/json");
            var httpResponseTask =
                await httpClient.PostAsync("http://localhost:3501/v1.0/publish/investments-orders/order-investment-created",
                    content);
        }

        private async Task PublishOrderRedeemCreated(Order newOrder)
        {
            var httpClient = _httpClientFactory.CreateClient("order");
            httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            var orderJson = JsonSerializer.Serialize(newOrder);

            var content = new StringContent(orderJson, Encoding.UTF8, "application/json");
            var httpResponseTask =
                await httpClient.PostAsync("http://localhost:3501/v1.0/publish/investments-orders/order-redeem-created",
                    content);
        }
        
        private async Task SaveStateAsync(Order newOrder)
        {
            var httpClient = _httpClientFactory.CreateClient("order");
            httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            var orderAsJson = JsonSerializer.Serialize(
                new[]
                {
                    new
                    {
                        key = newOrder.ToString(),
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