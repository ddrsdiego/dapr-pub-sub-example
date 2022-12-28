namespace Dapr.Sub.Repositories
{
    using System.Net.Http;
    using System.Net.Http.Headers;
    using System.Text;
    using System.Text.Json;
    using System.Threading.Tasks;
    using Microsoft.Extensions.Logging;
    using Models;

    public interface IOrderRepository
    {
        Task RegisterNewOrder(string stateName, Order order);
    }

    internal sealed class OrderRepository : IOrderRepository
    {
        private readonly ILogger<OrderRepository> _logger;
        private readonly IHttpClientFactory _httpClientFactory;

        public OrderRepository(ILogger<OrderRepository> logger, IHttpClientFactory httpClientFactory)
        {
            _logger = logger;
            _httpClientFactory = httpClientFactory;
        }

        public async Task RegisterNewOrder(string stateName, Order order)
        {
            var httpClient = _httpClientFactory.CreateClient(stateName);
            httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            var key = $"{stateName.ToLowerInvariant()}-{order.OrderId}";
            var orderAsJson = JsonSerializer.Serialize(
                new[]
                {
                    new
                    {
                        key = key,
                        value = order
                    }
                }
            );

            var state = new StringContent(orderAsJson, Encoding.UTF8, "application/json");
            var stateResponseMessage =
                await httpClient.PostAsync("http://localhost:3510/v1.0/state/dapr-pub--state-store", state);
        }
    }
}