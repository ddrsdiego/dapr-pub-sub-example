namespace Dapr.Pub.Controllers.Requests
{
    public class NewOrderInvestmentRequest
    {
        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }
    }
    
    public class NewOrderRedeemRequest
    {
        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }
    }
}