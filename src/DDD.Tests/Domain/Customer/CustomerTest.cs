using DDD.Catalog.Domain;
using DDD.Core.DomainObjects;
using FluentAssertions;
using System;
using System.Linq;
using Xunit;

namespace DDD.Tests.Domain.Customer;

[Collection(nameof(CustomerTestFixture))]
public class CustomerTest
{
    private readonly CustomerTestFixture _customerTestFixture;

    public CustomerTest(CustomerTestFixture customerTestFixture) => _customerTestFixture = customerTestFixture;

    [Fact(DisplayName = (nameof(Instanciate)))]
    [Trait("Domain", "Custumer - Agregates")]
    public void Instanciate()
    {
        //Arrange
        var validateCustomer = _customerTestFixture.GetValidCustomer();
        var dateTimeBefore = DateTime.Now;

        var address = new DDD.Customer.Domain.Address(
            _customerTestFixture.GetValidCustomerAddressStreet(),
            _customerTestFixture.GetValidCustomerAddressZipCode(),
            _customerTestFixture.GetValidCustomerAddressCity(),
             _customerTestFixture.GetValidCustomerAddressState(),
            validateCustomer.Id
        );

        // Act
        var customer = new DDD.Customer.Domain.Customer(validateCustomer.Name, validateCustomer.Email.Value, validateCustomer.Cpf.Number);
        customer.AssignAddress(address);

        var dateTimeAfter = DateTime.Now.AddSeconds(1);

        //Assert
        customer.Should().NotBeNull();
        customer.Name.Should().Be(validateCustomer.Name);
        customer.Email.Value.Should().Be(validateCustomer.Email.Value);
        customer.Cpf.Number.Should().Be(validateCustomer.Cpf.Number);
        customer.Id.Should().NotBe(default(Guid));
        customer.CreatedAt.Should().NotBeSameDateAs(default(DateTime));
        (customer.CreatedAt >= dateTimeBefore).Should().BeTrue();
        (customer.CreatedAt <= dateTimeAfter).Should().BeTrue();
        (customer.IsActive).Should().BeTrue();
    }

    [Theory(DisplayName = nameof(InstanciateWithIsActive))]
    [Trait("Domain", "Custumer - Agregates")]
    [InlineData(true)]
    [InlineData(false)]
    public void InstanciateWithIsActive(bool isActive)
    {
        //Arrange
        var validateCustomer = _customerTestFixture.GetValidCustomer();
        var dateTimeBefore = DateTime.Now;

        // Act
        var customer = new DDD.Customer.Domain.Customer(validateCustomer.Name, validateCustomer.Email.Value, validateCustomer.Cpf.Number, isActive);
        var dateTimeAfter = DateTime.Now.AddSeconds(1);

        //Assert
        customer.Should().NotBeNull();
        customer.Name.Should().Be(validateCustomer.Name);
        customer.Email.Value.Should().Be(validateCustomer.Email.Value);
        customer.Cpf.Number.Should().Be(validateCustomer.Cpf.Number);
        customer.Id.Should().NotBe(default(Guid));
        customer.CreatedAt.Should().NotBeSameDateAs(default(DateTime));
        (customer.CreatedAt >= dateTimeBefore).Should().BeTrue();
        (customer.CreatedAt <= dateTimeAfter).Should().BeTrue();
        (customer.IsActive).Should().Be(isActive);
    }

    [Theory(DisplayName = nameof(InstanceateErrorWhenNameIsEmpty))]
    [Trait("Domain", "Custumer - Agregates")]
    [InlineData("")]
    [InlineData(null)]
    [InlineData(" ")]
    public void InstanceateErrorWhenNameIsEmpty(string? name)
    {
        // Arrange
        var validateCustomer = _customerTestFixture.GetValidCustomer();
        Action action = () => new DDD.Customer.Domain.Customer(name!, validateCustomer.Email.Value, validateCustomer.Cpf.Number);

        //Act
        action.Should().Throw<DomainException>().WithMessage("Name should not be empty or null.");
    }

    [Theory(DisplayName = nameof(InstanceateErrorWhenNameIsLessThan3Characters))]
    [Trait("Domain", "Custumer - Agregates")]
    [InlineData("1")]
    [InlineData("12")]
    [InlineData("a")]
    [InlineData("ca")]
    public void InstanceateErrorWhenNameIsLessThan3Characters(string invalidName)
    {
        // Arrange
        var validateCustomer = _customerTestFixture.GetValidCustomer();
        Action action =
            () => new DDD.Customer.Domain.Customer(invalidName, validateCustomer.Email.Value, validateCustomer.Cpf.Number);

        //Act
        action.Should().Throw<DomainException>().WithMessage("Name should be at leats 3 characters.");

    }

    [Fact(DisplayName = nameof(InstanceateErrorWhenNameIsGreaterThan255Characters))]
    [Trait("Domain", "Custumer - Agregates")]
    public void InstanceateErrorWhenNameIsGreaterThan255Characters()
    {
        // Arrange
        var validateCustomer = _customerTestFixture.GetValidCustomer();
        var invalidName = String.Join(null, Enumerable.Range(1, 256).Select(_ => "a").ToArray());
        Action action =
            () => new DDD.Customer.Domain.Customer(invalidName, validateCustomer.Email.Value, validateCustomer.Cpf.Number);

        //Act
        action.Should().Throw<DomainException>().WithMessage("Name should be less or equal 255 characters long.");

    }

    [Fact(DisplayName = nameof(Activate))]
    [Trait("Domain", "Custumer - Agregates")]
    public void Activate()
    {
        //Arrange
        var validateCustomer = _customerTestFixture.GetValidCustomer();

        // Act
        var customer = new DDD.Customer.Domain.Customer(validateCustomer.Name, validateCustomer.Email.Value, validateCustomer.Cpf.Number, false);
        customer.Activate();

        //Assert
        customer.IsActive.Should().BeTrue();
    }

    [Fact(DisplayName = nameof(Deactivate))]
    [Trait("Domain", "Custumer - Agregates")]
    public void Deactivate()
    {
        //Arrange
        var validateCustomer = _customerTestFixture.GetValidCustomer();

        // Act
        var customer = new DDD.Customer.Domain.Customer(validateCustomer.Name, validateCustomer.Email.Value, validateCustomer.Cpf.Number, true);
        customer.Deactivate();

        //Assert
        customer.IsActive.Should().BeFalse();
    }

}
