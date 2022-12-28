namespace Dapr.Pub.Models
{
    using System;

    public sealed class Order
    {
        public Order(int quantity, decimal unitPrice)
            : this(Guid.NewGuid().ToString(), quantity, unitPrice)
        {
        }

        private Order(string orderId, int quantity, decimal unitPrice)
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