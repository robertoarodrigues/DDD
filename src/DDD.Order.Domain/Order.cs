using DDD.Core.DomainObjects;
using FluentValidation.Results;

namespace DDD.Order.Domain;
public class Order : Entity, IAggregateRoot
{

    public Order(Guid costumerId,
        bool voucherUsed,
        decimal discount,
        decimal amount)
    {
        CostumerId = costumerId;
        VoucherUsed = voucherUsed;
        Discount = discount;
        Total = amount;
        CreatedAt = DateTime.Now;
    }

    protected Order()
    {
        _orderItens = new List<OrderItem>();
    }

    public Guid CostumerId { get; private set; }
    public Guid? VoucherId { get; private set; }
    public bool VoucherUsed { get; private set; }
    public decimal Discount { get; private set; }
    public decimal Total { get; private set; }
    public OrderStatus OrderStatus { get; private set; }
    public DateTime CreatedAt { get; set; }

    private readonly List<OrderItem>?  _orderItens;
    public IReadOnlyCollection<OrderItem> OrderItems => _orderItens;
    public Voucher? Voucher { get; private set; }

    public ValidationResult ApplyVoucher(Voucher voucher)
    {
        var validationResult = voucher.ValidateIfApplicable();
        if (!validationResult.IsValid) return validationResult;

        Voucher = voucher;
        VoucherUsed = true;

        CalculateOrderValue();

        return validationResult;
    }

    public void CalculateOrderValue()
    {
        Total = OrderItems.Sum(p => p.CalculateValue());
        CalculateTotalValueDiscount();
    }

    public void CalculateTotalValueDiscount()
    {
        if (!VoucherUsed) return;

        decimal discount = 0;
        var value = Total;

        if (Voucher?.TypeDiscountVoucher == TypeDiscountVoucher.Percentage)
        {
            if (Voucher.Percentage.HasValue)
            {
                discount = (value * Voucher.Percentage.Value) / 100;
                value -= discount;
            }
        }
        else
        {
            if (Voucher.ValueDiscount.HasValue)
            {
                discount = Voucher.ValueDiscount.Value;
                value -= discount;
            }
        }

        Total = value < 0 ? 0 : value;
        Discount = discount;
    }

    public bool ExistingOrderItem(OrderItem item)
    {
        return _orderItens.Any(p => p.ProductId == item.ProductId);
    }

    public void AddItem(OrderItem item)
    {
        if (!item.IsValid()) return;

        item.AssociateOrder(Id);

        if (ExistingOrderItem(item))
        {
            var existingItem = _orderItens.FirstOrDefault(p => p.ProductId == item.ProductId);
            existingItem.AddUnits(item.Quantity);
            item = existingItem;

            _orderItens.Remove(existingItem);
        }

        item.CalculateValue();
        _orderItens.Add(item);

        CalculateOrderValue();
    }

    public void RemoveItem(OrderItem item)
    {
        if (!item.IsValid()) return;

        var existingItem = OrderItems.FirstOrDefault(p => p.ProductId == item.ProductId);

        if (existingItem == null) throw new DomainException("The item does not belong to the order");
        _orderItens.Remove(existingItem);

        CalculateOrderValue();
    }

    public void ChangeItem(OrderItem item)
    {
        if (!item.IsValid()) return;
        item.AssociateOrder(Id);

        var existingItem = OrderItems.FirstOrDefault(p => p.ProductId == item.ProductId);

        if (existingItem == null) throw new DomainException("The item does not belong to the order");

        _orderItens.Remove(existingItem);
        _orderItens.Add(item);

        CalculateOrderValue();
    }

    public void ChangeUnits(OrderItem item, int units)
    {
        item.ChangeUnits(units);
        ChangeItem(item);
    }

    public void MakeDraft()
    {
        OrderStatus = OrderStatus.Sketch;
    }

    public void StartOrder()
    {
        OrderStatus = OrderStatus.Initiate;
    }

    public void FinalizeOrder()
    {
        OrderStatus = OrderStatus.Paid;
    }

    public void CancelOrder()
    {
        OrderStatus = OrderStatus.Canceled;
    }
    public static class OrderFactory
    {
        public static Order NewOrderDraft(Guid customerId)
        {
            var order = new Order
            {
                CostumerId = customerId,
            };

            order.MakeDraft();
            return order;
        }
    }
}


