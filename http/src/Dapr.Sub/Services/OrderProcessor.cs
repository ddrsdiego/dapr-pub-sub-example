namespace Dapr.Sub.Services
{
    using System.Text.Json;
    using System.Threading.Tasks;
    using Microsoft.Extensions.Logging;
    using Models;

    public interface IOrderProcessor
    {
        Task Process(Order order);
    }

    internal sealed class OrderProcessor : IOrderProcessor
    {
        private readonly ILogger<OrderProcessor> _logger;

        public OrderProcessor(ILogger<OrderProcessor> logger)
        {
            _logger = logger;
        }
        
        public async Task Process(Order order)
        {
            _logger.LogInformation($"Order Processed: {JsonSerializer.Serialize(order)}");
            
            await Task.CompletedTask;
        }
    }
}