using DDD.Core.DomainObjects;
using FluentAssertions;
using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace DDD.Tests.Domain.Product;

[Collection(nameof(ProductTestFixture))]
public class ProductTest
{
    private readonly ProductTestFixture _productTestFixture;

    public ProductTest(ProductTestFixture productTestFixture) => _productTestFixture = productTestFixture;

    [Fact(DisplayName = (nameof(Instanciate)))]
    [Trait("Domain", "Product - Agregates")]
    public void Instanciate()
    {
        //Arrange
        var validateProduct = _productTestFixture.GetValidProduct();
        var dateTimeBefore = DateTime.Now;

        // Act
        var product = new Catalog.Domain.Product(validateProduct.CategoryId, validateProduct.Name, validateProduct.Description, validateProduct.Price);
        var dateTimeAfter = DateTime.Now.AddSeconds(1);

        //Assert
        product.Should().NotBeNull();
        product.Name.Should().Be(validateProduct.Name);
        product.Description.Should().Be(validateProduct.Description);
        product.Id.Should().NotBe(default(Guid));
        product.CreatedAt.Should().NotBeSameDateAs(default(DateTime));
        (product.CreatedAt >= dateTimeBefore).Should().BeTrue();
        (product.CreatedAt <= dateTimeAfter).Should().BeTrue();
        (product.IsActive).Should().BeTrue();
    }

    [Theory(DisplayName = nameof(InstanciateWithIsActive))]
    [Trait("Domain", "Product - Agregates")]
    [InlineData(true)]
    [InlineData(false)]
    public void InstanciateWithIsActive(bool isActive)
    {
        //Arrange
        var validateProduct = _productTestFixture.GetValidProduct();
        var dateTimeBefore = DateTime.Now;

        // Act
        var product = new Catalog.Domain.Product(validateProduct.CategoryId, validateProduct.Name, validateProduct.Description, validateProduct.Price, isActive);
        var dateTimeAfter = DateTime.Now.AddSeconds(1);

        //Assert
        product.Should().NotBeNull();
        product.Name.Should().Be(validateProduct.Name);
        product.Description.Should().Be(validateProduct.Description);
        product.Id.Should().NotBe(default(Guid));
        product.CreatedAt.Should().NotBeSameDateAs(default(DateTime));
        (product.CreatedAt >= dateTimeBefore).Should().BeTrue();
        (product.CreatedAt <= dateTimeAfter).Should().BeTrue();
        (product.IsActive).Should().Be(isActive);
    }

    [Theory(DisplayName = nameof(InstanceateErrorWhenNameIsEmpty))]
    [Trait("Domain", "Product - Agregates")]
    [InlineData("")]
    [InlineData(null)]
    [InlineData(" ")]
    public void InstanceateErrorWhenNameIsEmpty(string? name)
    {
        // Arrange
        var validateProduct = _productTestFixture.GetValidProduct();
        Action action = () => new Catalog.Domain.Product(validateProduct.CategoryId, name!, validateProduct.Description, validateProduct.Price);

        //Act
        action.Should().Throw<DomainException>().WithMessage("Name should not be empty or null.");
    }

    [Fact(DisplayName = nameof(InstanceateErrorWhenDescriptionIsNull))]
    [Trait("Domain", "Product - Agregates")]
    [InlineData(null)]
    public void InstanceateErrorWhenDescriptionIsNull()
    {
        // Arrange
        var validateProduct = _productTestFixture.GetValidProduct();
        Action action =
            () => new Catalog.Domain.Product(validateProduct.CategoryId, validateProduct.Name, null!, validateProduct.Price);

        //Act
        action.Should().Throw<DomainException>().WithMessage("Description should be not be null.");
    }

    [Theory(DisplayName = nameof(InstanceateErrorWhenNameIsLessThan3Characters))]
    [Trait("Domain", "Product - Agregates")]
    [InlineData("1")]
    [InlineData("12")]
    [InlineData("a")]
    [InlineData("ca")]
    public void InstanceateErrorWhenNameIsLessThan3Characters(string invalidName)
    {
        // Arrange
        var validateProduct = _productTestFixture.GetValidProduct();
        Action action =
            () => new Catalog.Domain.Product(validateProduct.CategoryId, invalidName, validateProduct.Description, validateProduct.Price);

        //Act
        action.Should().Throw<DomainException>().WithMessage("Name should be at leats 3 characters.");

    }

    [Fact(DisplayName = nameof(InstanceateErrorWhenNameIsGreaterThan255Characters))]
    [Trait("Domain", "Product - Agregates")]
    public void InstanceateErrorWhenNameIsGreaterThan255Characters()
    {
        // Arrange
        var validateProduct = _productTestFixture.GetValidProduct();
        var invalidName = String.Join(null, Enumerable.Range(1, 256).Select(_ => "a").ToArray());
        Action action =
            () => new Catalog.Domain.Product(validateProduct.CategoryId, invalidName, validateProduct.Description, validateProduct.Price);

        //Act
        action.Should().Throw<DomainException>().WithMessage("Name should be less or equal 255 characters long.");

    }

    [Fact(DisplayName = nameof(InstanceateErrorWhenDescripionIsGreaterThan10000Characters))]
    [Trait("Domain", "Product - Agregates")]
    public void InstanceateErrorWhenDescripionIsGreaterThan10000Characters()
    {
        // Arrange
        var validateProduct = _productTestFixture.GetValidProduct();
        var invalidDescription = String.Join(null, Enumerable.Range(1, 10001).Select(_ => "a").ToArray());
        Action action =
            () => new Catalog.Domain.Product(validateProduct.CategoryId, validateProduct.Name, invalidDescription, validateProduct.Price);

        //Act
        action.Should().Throw<DomainException>().WithMessage("Description should be less or equal 10000 characters long.");
    }

    [Fact(DisplayName = nameof(Activate))]
    [Trait("Domain", "Product - Agregates")]
    public void Activate()
    {
        //Arrange
        var validateProduct = _productTestFixture.GetValidProduct();

        // Act
        var product = new Catalog.Domain.Product(validateProduct.CategoryId, validateProduct.Name, validateProduct.Description, validateProduct.Price, false);
        product.Activate();

        //Assert
        product.IsActive.Should().BeTrue();
    }

    [Fact(DisplayName = nameof(Deactivate))]
    [Trait("Domain", "Product - Agregates")]
    public void Deactivate()
    {
        //Arrange
        var validateProduct = _productTestFixture.GetValidProduct();

        // Act
        var product = new Catalog.Domain.Product(validateProduct.CategoryId, validateProduct.Name, validateProduct.Description, validateProduct.Price, true);
        product.Deactivate();

        //Assert
        product.IsActive.Should().BeFalse();
    }

    [Fact(DisplayName = nameof(Update))]
    [Trait("Domain", "Product - Agregates")]
    public void Update()
    {
        var product = _productTestFixture.GetValidProduct();
        var productyWithNewValues = _productTestFixture.GetValidProduct();

        product.Update(productyWithNewValues.Name, productyWithNewValues.Description, productyWithNewValues.Price);

        product.Name.Should().Be(productyWithNewValues.Name);
        product.Description.Should().Be(productyWithNewValues.Description);
        product.Price.Should().Be(productyWithNewValues.Price);
    }

    [Fact(DisplayName = nameof(UpdateOnlyName))]
    [Trait("Domain", "Product - Agregates")]
    public void UpdateOnlyName()
    {
        var product = _productTestFixture.GetValidProduct();
        var newName = _productTestFixture.GetValidProductName();
        var currentDescription = product.Description;

        product.Update(newName);

        product.Name.Should().Be(newName);
        product.Description.Should().Be(currentDescription);
    }

    [Theory(DisplayName = nameof(UpadateErrorWhenNameIsEmpty))]
    [Trait("Domain", "Product - Agregates")]
    [InlineData("")]
    [InlineData(null)]
    [InlineData(" ")]
    public void UpadateErrorWhenNameIsEmpty(string? name)
    {
        // Arrange
        var product = _productTestFixture.GetValidProduct();

        Action action =
            () => product.Update(name!);

        //Act
        action.Should().Throw<DomainException>().WithMessage("Name should not be empty or null.");
    }

    [Theory(DisplayName = nameof(UpadateErrorWhenNameIsLessThan3Characters))]
    [Trait("Domain", "Product - Agregates")]
    [MemberData(nameof(GetNamesWithLessThan3Caharacters), parameters: 10)]
    public void UpadateErrorWhenNameIsLessThan3Characters(string invalidName)
    {
        // Arrange
        var product = _productTestFixture.GetValidProduct();
        Action action =
            () => product.Update(invalidName);

        //Act
        action.Should().Throw<DomainException>().WithMessage("Name should be at leats 3 characters.");
    }

    public static IEnumerable<object[]> GetNamesWithLessThan3Caharacters(int numberOfTests = 6)
    {
        var fixture = new ProductTestFixture();

        for (int i = 0; i < numberOfTests; i++)
        {
            var isOdd = i % 2 == 1;

            yield return new object[] { fixture.GetValidProductName()[..(isOdd ? 1 : 2)] };
        }
    }

    [Fact(DisplayName = nameof(UpadateErrorWhenNameIsGreaterThan255Characters))]
    [Trait("Domain", "Product - Agregates")]
    public void UpadateErrorWhenNameIsGreaterThan255Characters()
    {
        // Arrange
        var product = _productTestFixture.GetValidProduct();
        var invalidName = _productTestFixture.Faker.Lorem.Letter(256);
        Action action =
            () => product.Update(invalidName);

        //Act
        action.Should().Throw<DomainException>().WithMessage("Name should be less or equal 255 characters long.");
    }

    [Fact(DisplayName = nameof(UpadateErrorWhenDescripionIsGreaterThan10000Characters))]
    [Trait("Domain", "Product - Agregates")]
    public void UpadateErrorWhenDescripionIsGreaterThan10000Characters()
    {
        // Arrange
        var product = _productTestFixture.GetValidProduct();

        var invalidDescription = _productTestFixture.Faker.Commerce.ProductDescription();

        while (invalidDescription.Length <= 10000)
            invalidDescription = $"{invalidDescription} {_productTestFixture.Faker.Commerce.ProductDescription()}";

        Action action =
            () => product.Update("Product Name", invalidDescription);

        //Act
        action.Should().Throw<DomainException>().WithMessage("Description should be less or equal 10000 characters long.");
    }
}
