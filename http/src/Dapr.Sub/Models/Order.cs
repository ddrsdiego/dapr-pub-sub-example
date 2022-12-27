namespace Dapr.Sub.Models
{
    public class Order
    {
        public string OrderId { get; set; }
        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }
    }
}