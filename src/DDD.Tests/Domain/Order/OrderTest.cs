using DDD.Core.DomainObjects;
using DDD.Order.Domain;
using System;
using System.Linq;
using Xunit;

namespace DDD.Tests.Domain.Order;
public class OrderTest
{
    [Fact(DisplayName = (nameof(Instanciate)))]
    [Trait("Domain", "Order - Agregates")]
    public void Instanciate()
    {
        // Arrange
        var order = DDD.Order.Domain.Order.OrderFactory.NewOrderDraft(Guid.NewGuid());
        var orderItem = new OrderItem(Guid.NewGuid(), "Product Test", 2, 100);

        // Act
        order.AddItem(orderItem);

        // Assert
        Assert.Equal(200, order.Total);
    }

    [Fact(DisplayName = nameof(AddOrderItem_ItemExistente_MustIncrementUnitsSumValues))]
    [Trait("Domain", "Order - Agregates")]
    public void AddOrderItem_ItemExistente_MustIncrementUnitsSumValues()
    {
        // Arrange
        var order = DDD.Order.Domain.Order.OrderFactory.NewOrderDraft(Guid.NewGuid());
        var productId = Guid.NewGuid();
        var orderItem = new OrderItem(productId, "Product Test", 2, 100);
        order.AddItem(orderItem);

        var orderItem2 = new OrderItem(productId, "Product Test", 1, 100);

        // Act
        order.AddItem(orderItem2);

        // Assert
        Assert.Equal(300, order.Total);
        Assert.Equal(1, order.OrderItems.Count);
        Assert.Equal(3, order.OrderItems.FirstOrDefault(p => p.ProductId == productId).Quantity);
    }

    [Fact(DisplayName = nameof(UpdateOrderItem_ItemNotExistemAtList_MustReturnException))]
    [Trait("Domain", "Order - Agregates")]
    public void UpdateOrderItem_ItemNotExistemAtList_MustReturnException()
    {
        // Arrange
        var order = DDD.Order.Domain.Order.OrderFactory.NewOrderDraft(Guid.NewGuid());
        var orderItemUpdated = new OrderItem(Guid.NewGuid(), "Product Test", 5, 100);

        // Act & Assert
        Assert.Throws<DomainException>(() => order.ChangeItem(orderItemUpdated));
    }

    [Fact(DisplayName = nameof(UpdateOrderItem_ItemValid))]
    [Trait("Domain", "Order - Agregates")]
    public void UpdateOrderItem_ItemValid()
    {
        // Arrange
        var order = DDD.Order.Domain.Order.OrderFactory.NewOrderDraft(Guid.NewGuid());
        var productId = Guid.NewGuid();
        var orderItem = new OrderItem(productId, "Product Test", 2, 100);
        order.AddItem(orderItem);
        var orderItemUpdated = new OrderItem(productId, "Product Test", 5, 100);
        var newQuantity = orderItemUpdated.Quantity;

        // Act
        order.ChangeItem(orderItemUpdated);

        // Assert
        Assert.Equal(newQuantity, order.OrderItems.FirstOrDefault(p => p.ProductId == productId).Quantity);
    }

    [Fact(DisplayName = nameof(UpdateOrderItem_OrderWithProductsDifferent_MustUpdateTotalValue))]
    [Trait("Domain", "Order - Agregates")]
    public void UpdateOrderItem_OrderWithProductsDifferent_MustUpdateTotalValue()
    {
        // Arrange
        var order = DDD.Order.Domain.Order.OrderFactory.NewOrderDraft(Guid.NewGuid());
        var productId = Guid.NewGuid();
        var orderExistingItem1 = new OrderItem(Guid.NewGuid(), "Product Xpto", 2, 100);
        var orderExistingItem2 = new OrderItem(productId, "Product Teste", 3, 15);
        order.AddItem(orderExistingItem1);
        order.AddItem(orderExistingItem2);

        var orderItemUpdated = new OrderItem(productId, "Product Teste", 5, 15);
        var totalOrder = orderExistingItem1.Quantity * orderExistingItem1.UnitPrice +
                         orderItemUpdated.Quantity * orderItemUpdated.UnitPrice;

        // Act
        order.ChangeItem(orderItemUpdated);

        // Assert
        Assert.Equal(totalOrder, order.Total);
    }

    [Fact(DisplayName = nameof(RemoveOrderItem_ItemNotExisteNaList_MustRetornarException))]
    [Trait("Domain", "Order - Agregates")]
    public void RemoveOrderItem_ItemNotExisteNaList_MustRetornarException()
    {
        // Arrange
        var order = DDD.Order.Domain.Order.OrderFactory.NewOrderDraft(Guid.NewGuid());
        var orderItemRemove = new OrderItem(Guid.NewGuid(), "Product Teste", 5, 100);

        // Act & Assert
        Assert.Throws<DomainException>(() => order.RemoveItem(orderItemRemove));
    }

    [Fact(DisplayName = nameof(RemoveOrderItem_ItemExistente_MustUpdateTotalValue))]
    [Trait("Domain", "Order - Agregates")]
    public void RemoveOrderItem_ItemExistente_MustUpdateTotalValue()
    {
        // Arrange
        var order = DDD.Order.Domain.Order.OrderFactory.NewOrderDraft(Guid.NewGuid());
        var productId = Guid.NewGuid();
        var orderItem1 = new OrderItem(Guid.NewGuid(), "Product Xpto", 2, 100);
        var orderItem2 = new OrderItem(productId, "Product Teste", 3, 15);
        order.AddItem(orderItem1);
        order.AddItem(orderItem2);

        var totalPedido = orderItem2.Quantity * orderItem2.UnitPrice;

        // Act
        order.RemoveItem(orderItem1);

        // Assert
        Assert.Equal(totalPedido, order.Total);
    }

    [Fact(DisplayName = nameof(Order_AplicationVoucherValid_MustReturnWithoutErrors))]
    [Trait("Domain", "Order - Agregates")]
    public void Order_AplicationVoucherValid_MustReturnWithoutErrors()
    {
        // Arrange
        var order = DDD.Order.Domain.Order.OrderFactory.NewOrderDraft(Guid.NewGuid());
        var voucher = new Voucher("PROMO-15-REAIS", null, 15, 1,
            TypeDiscountVoucher.Value, DateTime.Now.AddDays(15), true, false);

        // Act
        var result = order.ApplyVoucher(voucher);

        // Assert
        Assert.True(result.IsValid);
    }

    [Fact(DisplayName = nameof(Order_ApplyVoucherInvalid_MustReturnWhithErros))]
    [Trait("Domain", "Order - Agregates")]
    public void Order_ApplyVoucherInvalid_MustReturnWhithErros()
    {
        // Arrange
        var order = DDD.Order.Domain.Order.OrderFactory.NewOrderDraft(Guid.NewGuid());
        var voucher = new Voucher("PROMO-15-REAIS", null, 15, 1,
            TypeDiscountVoucher.Value, DateTime.Now.AddDays(-1), true, true);

        // Act
        var result = order.ApplyVoucher(voucher);

        // Assert
        Assert.False(result.IsValid);
    }

    [Fact(DisplayName = nameof(ApplyVoucher_VoucherTypeValorDiscount_MustDiscountValueTotal))]
    [Trait("Domain", "Order - Agregates")]
    public void ApplyVoucher_VoucherTypeValorDiscount_MustDiscountValueTotal()
    {
        // Arrange
        var order = DDD.Order.Domain.Order.OrderFactory.NewOrderDraft(Guid.NewGuid());

        var orderItem1 = new OrderItem(Guid.NewGuid(), "Product Xpto", 2, 100);
        var orderItem2 = new OrderItem(Guid.NewGuid(), "Product Teste", 3, 15);
        order.AddItem(orderItem1);
        order.AddItem(orderItem2);

        var voucher = new Voucher("PROMO-15-REAIS", null, 15, 1,
            TypeDiscountVoucher.Value, DateTime.Now.AddDays(10), true, false);

        var valorComDesconto = order.Total - voucher.ValueDiscount;

        // Act
        order.ApplyVoucher(voucher);

        // Assert
        Assert.Equal(valorComDesconto, order.Total);
    }

    [Fact(DisplayName = nameof(ApplyVoucher_VoucherTypePercentualDiscount_MustDiscountValueTotal))]
    [Trait("Domain", "Order - Agregates")]
    public void ApplyVoucher_VoucherTypePercentualDiscount_MustDiscountValueTotal()
    {
        // Arrange
        var order = DDD.Order.Domain.Order.OrderFactory.NewOrderDraft(Guid.NewGuid());

        var orderItem1 = new OrderItem(Guid.NewGuid(), "Product Xpto", 2, 100);
        var orderItem2 = new OrderItem(Guid.NewGuid(), "Product Teste", 3, 15);
        order.AddItem(orderItem1);
        order.AddItem(orderItem2);

        var voucher = new Voucher("PROMO-15-OFF", 15, null, 1,
            TypeDiscountVoucher.Percentage, DateTime.Now.AddDays(10), true, false);

        var valueDiscount = (order.Total * voucher.Percentage) / 100;
        var valorTotalComDesconto = order.Total - valueDiscount;

        // Act
        order.ApplyVoucher(voucher);

        // Assert
        Assert.Equal(valorTotalComDesconto, order.Total);
    }

    [Fact(DisplayName = nameof(ApplyVoucher_DiscountExcedeTotalValueOrder_OrderMustHaveValueZero))]
    [Trait("Domain", "Order - Agregates")]
    public void ApplyVoucher_DiscountExcedeTotalValueOrder_OrderMustHaveValueZero()
    {
        // Arrange
        var order = DDD.Order.Domain.Order.OrderFactory.NewOrderDraft(Guid.NewGuid());

        var orderItem1 = new OrderItem(Guid.NewGuid(), "Product Xpto", 2, 100);
        order.AddItem(orderItem1);

        var voucher = new Voucher("PROMO-15-OFF", null, 300, 1,
            TypeDiscountVoucher.Value, DateTime.Now.AddDays(10), true, false);

        // Act
        order.ApplyVoucher(voucher);

        // Assert
        Assert.Equal(0, order.Total);
    }

    [Fact(DisplayName = nameof(ApplyVoucher_ModifyItemsOrder_MustCalculateDiscountTotalValue))]
    [Trait("Domain", "Order - Agregates")]
    public void ApplyVoucher_ModifyItemsOrder_MustCalculateDiscountTotalValue()
    {
        // Arrange
        var order = DDD.Order.Domain.Order.OrderFactory.NewOrderDraft(Guid.NewGuid());
        var orderItem1 = new OrderItem(Guid.NewGuid(), "Product Xpto", 2, 100);
        order.AddItem(orderItem1);

        var voucher = new Voucher("PROMO-15-OFF", null, 50, 1,
            TypeDiscountVoucher.Value, DateTime.Now.AddDays(10), true, false);
        
        order.ApplyVoucher(voucher);

        var orderItem2 = new OrderItem(Guid.NewGuid(), "Product Teste", 4, 25);

        // Act
        order.AddItem(orderItem2);

        // Assert
        var totalExpected = order.OrderItems.Sum(i => i.Quantity * i.UnitPrice) - voucher.ValueDiscount;
        Assert.Equal(totalExpected, order.Total);
    }
}
