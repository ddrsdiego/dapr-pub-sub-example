namespace Dapr.Sub.Controllers
{
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Mvc;
    using Models;
    using Services;

    [ApiController]
    [Route("/")]
    public sealed class OrderRedeemCreatedConsumerController : ControllerBase
    {
        private readonly IOrderProcessor _orderProcessor;

        public OrderRedeemCreatedConsumerController(IOrderProcessor orderProcessor)
        {
            _orderProcessor = orderProcessor;
        }

        [HttpPost("order-redeem-created")]
        public async Task<IActionResult> ReceiveOrderRedeemCreated([FromBody] DaprData<Order> message)
        {
            await _orderProcessor.Process("order-redeem", message.Data);
            return Ok();
        }
    }
}