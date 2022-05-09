using DDD.Core.DomainObjects;

namespace DDD.Customer.Domain;
public class Address : Entity
{
    public Address(string street, string zipCode, string city, string state, Guid customerId)
    {
        Street = street;
        ZipCode = zipCode;
        City = city;
        State = state;
        CustomerId = customerId;
    }

    public string Street { get; private set; }
    public string ZipCode { get; private set; }
    public string City { get; private set; }
    public string State { get; private set; }
    public Guid CustomerId { get; private set; }
}
