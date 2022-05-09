using DDD.Core.DomainObjects;
using FluentValidation;
using FluentValidation.Results;

namespace DDD.Order.Domain;
public class Voucher : Entity
{
    public Voucher(string code, decimal? percentage, decimal? valueDiscount, int quantity, TypeDiscountVoucher typeDiscountVoucher, DateTime expirationDate, bool isAtivo, bool used)
    {
        Code = code;
        Percentage = percentage;
        ValueDiscount = valueDiscount;
        Quantity = quantity;
        TypeDiscountVoucher = typeDiscountVoucher;
        ExpirationDate = expirationDate;
        IsAtivo = isAtivo;
        Used = used;
    }

    public string Code { get; private set; }
    public decimal? Percentage { get; private set; }
    public decimal? ValueDiscount { get; private set; }
    public int Quantity { get; private set; }
    public TypeDiscountVoucher TypeDiscountVoucher { get; private set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? DateOfUse { get; private set; }
    public DateTime ExpirationDate { get; private set; }
    public bool IsAtivo { get; private set; }
    public bool Used { get; private set; }

    internal ValidationResult ValidateIfApplicable()
    {
        return new VoucherAplicavelValidation().Validate(this);
    }
}
public class VoucherAplicavelValidation : AbstractValidator<Voucher>
{
    public VoucherAplicavelValidation()
    {
        RuleFor(c => c.ExpirationDate)
            .Must(ExpirationDateSuperiorCurrent)
            .WithMessage("This voucher is expired.");

        RuleFor(c => c.IsAtivo)
            .Equal(true)
            .WithMessage("This voucher is no longer valid.");

        RuleFor(c => c.Used)
            .Equal(false)
            .WithMessage("This voucher has already been used.");

        RuleFor(c => c.Quantity)
            .GreaterThan(0)
            .WithMessage("This voucher is no longer available.");
    }

    protected static bool ExpirationDateSuperiorCurrent(DateTime expirationDate)
    {
        return expirationDate >= DateTime.Now;
    }
}


