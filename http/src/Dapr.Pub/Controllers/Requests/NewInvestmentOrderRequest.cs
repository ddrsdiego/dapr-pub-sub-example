namespace Dapr.Pub.Controllers.Requests
{
    public class NewInvestmentOrderRequest
    {
        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }
    }
}