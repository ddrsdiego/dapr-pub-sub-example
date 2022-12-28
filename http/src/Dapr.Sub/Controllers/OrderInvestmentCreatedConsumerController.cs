namespace Dapr.Sub.Controllers
{
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Mvc;
    using Models;
    using Services;

    [ApiController]
    [Route("/")]
    public sealed class OrderInvestmentCreatedConsumerController : ControllerBase
    {
        private readonly IOrderProcessor _orderProcessor;

        public OrderInvestmentCreatedConsumerController(IOrderProcessor orderProcessor)
        {
            _orderProcessor = orderProcessor;
        }

        [HttpPost("order-investment-created")]
        public async Task<IActionResult> Receive([FromBody] DaprData<Order> message)
        {
            await _orderProcessor.Process("order-investment", message.Data);
            return Ok();
        }
    }
}