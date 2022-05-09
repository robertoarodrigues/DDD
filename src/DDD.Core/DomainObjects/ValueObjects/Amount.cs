namespace DDD.Core.DomainObjects.ValueObjects;
public sealed record Amount(decimal Value)
{
    public override string ToString() => Value.ToString();
}
