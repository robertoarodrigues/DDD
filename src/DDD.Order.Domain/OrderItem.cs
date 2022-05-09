using DDD.Core.DomainObjects;
using DDD.Core.DomainObjects.ValueObjects;

namespace DDD.Order.Domain;
public class OrderItem : Entity
{
    public OrderItem(Guid productId, string productName, int quantity, decimal unitPrice)
    {
        ProductId = productId;
        ProductName = productName;
        Quantity = quantity;
        UnitPrice = unitPrice;
        CreatedAt = DateTime.Now;
    }

    public Guid OrderId { get; private set; }
    public Guid ProductId { get; private set; }
    public string ProductName { get; private set; }
    public int Quantity { get; private set; }
    public decimal UnitPrice { get; private set; }
    public DateTime CreatedAt { get; set; }

    internal void AssociateOrder(Guid orderId)
    {
       OrderId = orderId;
    }

    public decimal CalculateValue()
    {
        return Quantity * UnitPrice;
    }

    internal void AddUnits(int unit)
    {
        Quantity += unit;
    }

    internal void ChangeUnits(int unit)
    {
        Quantity = unit;
    }

    public override bool IsValid()
    {
        return true;
    }
}
