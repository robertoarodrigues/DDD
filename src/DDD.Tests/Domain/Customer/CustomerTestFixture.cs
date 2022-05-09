using Bogus.DataSets;
using Bogus.Extensions.Brazil;
using DDD.Tests.Common;
using Xunit;

namespace DDD.Tests.Domain.Customer;
public class CustomerTestFixture : BasicFixture
{
    public CustomerTestFixture() : base() { }

    public string GetValidCustomerName()
    {
        var customerName = string.Empty;

        while (customerName.Length < 3)
            customerName = Faker.Person.UserName;
        if (customerName.Length > 255)
            customerName = customerName[..255];

        return customerName;
    }

    public string GetValidCustomerEmail()
    {
        var customerEmail = Faker.Internet.Email();

        return customerEmail;
    }

    public string GetValidCustomerCpf()
    {
        var customerEmail = Faker.Person.Cpf();

        return customerEmail;
    }

    public string GetValidCustomerAddressCity()
    {
        var customerAddressCity = Faker.Address.City();
        return customerAddressCity;
    }

    public string GetValidCustomerAddressState()
    {
        var customerAddressState = Faker.Address.State();
        return customerAddressState;
    }

    public string GetValidCustomerAddressStreet()
    {
        var customerAddressStreet = Faker.Address.StreetName();
        return customerAddressStreet;
    }

    public string GetValidCustomerAddressZipCode()
    {
        var customerAddressZipCode = Faker.Address.ZipCode();
        return customerAddressZipCode;
    }

    public DDD.Customer.Domain.Customer GetValidCustomer()
        => new(GetValidCustomerName(), GetValidCustomerEmail(), GetValidCustomerCpf());
}

[CollectionDefinition(nameof(CustomerTestFixture))]
public class CustomerTestFixtureCollection : ICollectionFixture<CustomerTestFixture>
{
}

