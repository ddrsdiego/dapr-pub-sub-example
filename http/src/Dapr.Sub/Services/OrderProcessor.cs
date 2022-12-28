namespace Dapr.Sub.Services
{
    using System.Net.Http;
    using System.Text.Json;
    using System.Threading.Tasks;
    using Microsoft.Extensions.Logging;
    using Models;
    using Repositories;

    public interface IOrderProcessor
    {
        Task Process(string stateName, Order order);
    }

    internal sealed class OrderProcessor : IOrderProcessor
    {
        private readonly ILogger<OrderProcessor> _logger;
        private readonly IOrderRepository _orderRepository;

        public OrderProcessor(ILogger<OrderProcessor> logger, IOrderRepository orderRepository)
        {
            _logger = logger;
            _orderRepository = orderRepository;
        }

        public async Task Process(string stateName, Order order)
        {
            _logger.LogInformation($"Order {stateName} Processed: {JsonSerializer.Serialize(order)}");

            await _orderRepository.RegisterNewOrder(stateName, order);

            await Task.CompletedTask;
        }
    }
}