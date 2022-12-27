namespace Dapr.Pub.Models
{
    public class Order
    {
        public Order(string orderId, int quantity, decimal unitPrice)
        {
            OrderId = orderId;
            Quantity = quantity;
            UnitPrice = unitPrice;
        }
        
        public string OrderId { get; }
        public int Quantity { get; }
        public decimal UnitPrice { get; }
    }
}