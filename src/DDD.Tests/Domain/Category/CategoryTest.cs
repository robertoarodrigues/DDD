using DDD.Catalog.Domain;
using DDD.Core.DomainObjects;
using FluentAssertions;
using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace DDD.Tests.Domain;

[Collection(nameof(CategoryTestFixture))]
public class CategoryTest
{
    private readonly CategoryTestFixture _categoryTestFixture;

    public CategoryTest(CategoryTestFixture categoryTestFixture) => _categoryTestFixture = categoryTestFixture;

    [Fact(DisplayName = (nameof(Instanciate)))]
    [Trait("Domain", "Category - Agregates")]
    public void Instanciate()
    {
        //Arrange
        var validateCategory = _categoryTestFixture.GetValidCategory();
        var dateTimeBefore = DateTime.Now;

        // Act
        var category = new Category(validateCategory.Name, validateCategory.Description);
        var dateTimeAfter = DateTime.Now.AddSeconds(1);

        //Assert
        category.Should().NotBeNull();
        category.Name.Should().Be(validateCategory.Name);
        category.Description.Should().Be(validateCategory.Description);
        category.Id.Should().NotBe(default(Guid));
        category.CreatedAt.Should().NotBeSameDateAs(default(DateTime));
        (category.CreatedAt >= dateTimeBefore).Should().BeTrue();
        (category.CreatedAt <= dateTimeAfter).Should().BeTrue();
        (category.IsActive).Should().BeTrue();
    }

    [Theory(DisplayName = nameof(InstanciateWithIsActive))]
    [Trait("Domain", "Category - Agregates")]
    [InlineData(true)]
    [InlineData(false)]
    public void InstanciateWithIsActive(bool isActive)
    {
        //Arrange
        var validateCategory = _categoryTestFixture.GetValidCategory();
        var dateTimeBefore = DateTime.Now;

        // Act
        var category = new Category(validateCategory.Name, validateCategory.Description, isActive);
        var dateTimeAfter = DateTime.Now.AddSeconds(1);

        //Assert
        category.Should().NotBeNull();
        category.Name.Should().Be(validateCategory.Name);
        category.Description.Should().Be(validateCategory.Description);
        category.Id.Should().NotBe(default(Guid));
        category.CreatedAt.Should().NotBeSameDateAs(default(DateTime));
        (category.CreatedAt >= dateTimeBefore).Should().BeTrue();
        (category.CreatedAt <= dateTimeAfter).Should().BeTrue();
        (category.IsActive).Should().Be(isActive);
    }

    [Theory(DisplayName = nameof(InstanceateErrorWhenNameIsEmpty))]
    [Trait("Domain", "Category - Agregates")]
    [InlineData("")]
    [InlineData(null)]
    [InlineData(" ")]
    public void InstanceateErrorWhenNameIsEmpty(string? name)
    {
        // Arrange
        var validateCategory = _categoryTestFixture.GetValidCategory();
        Action action = () => new Category(name!, validateCategory.Description);

        //Act
        action.Should().Throw<DomainException>().WithMessage("Name should not be empty or null.");
    }

    [Fact(DisplayName = nameof(InstanceateErrorWhenDescriptionIsNull))]
    [Trait("Domain", "Category - Agregates")]
    [InlineData(null)]
    public void InstanceateErrorWhenDescriptionIsNull()
    {
        // Arrange
        var validateCategory = _categoryTestFixture.GetValidCategory();
        Action action =
            () => new Category(validateCategory.Name, null!);

        //Act
        action.Should().Throw<DomainException>().WithMessage("Description should be not be null.");
    }

    [Theory(DisplayName = nameof(InstanceateErrorWhenNameIsLessThan3Characters))]
    [Trait("Domain", "Category - Agregates")]
    [InlineData("1")]
    [InlineData("12")]
    [InlineData("a")]
    [InlineData("ca")]
    public void InstanceateErrorWhenNameIsLessThan3Characters(string invalidName)
    {
        // Arrange
        var validateCategory = _categoryTestFixture.GetValidCategory();
        Action action =
            () => new Category(invalidName, validateCategory.Description);

        //Act
        action.Should().Throw<DomainException>().WithMessage("Name should be at leats 3 characters.");

    }

    [Fact(DisplayName = nameof(InstanceateErrorWhenNameIsGreaterThan255Characters))]
    [Trait("Domain", "Category - Agregates")]
    public void InstanceateErrorWhenNameIsGreaterThan255Characters()
    {
        // Arrange
        var validateCategory = _categoryTestFixture.GetValidCategory();
        var invalidName = String.Join(null, Enumerable.Range(1, 256).Select(_ => "a").ToArray());
        Action action =
            () => new Category(invalidName, validateCategory.Description);

        //Act
        action.Should().Throw<DomainException>().WithMessage("Name should be less or equal 255 characters long.");

    }

    [Fact(DisplayName = nameof(InstanceateErrorWhenDescripionIsGreaterThan10000Characters))]
    [Trait("Domain", "Category - Agregates")]
    public void InstanceateErrorWhenDescripionIsGreaterThan10000Characters()
    {
        // Arrange
        var validateCategory = _categoryTestFixture.GetValidCategory();
        var invalidDescription = String.Join(null, Enumerable.Range(1, 10001).Select(_ => "a").ToArray());
        Action action =
            () => new Category(validateCategory.Name, invalidDescription);

        //Act
        action.Should().Throw<DomainException>().WithMessage("Description should be less or equal 10000 characters long.");
    }

    [Fact(DisplayName = nameof(Activate))]
    [Trait("Domain", "Category - Agregates")]
    public void Activate()
    {
        //Arrange
        var validateCategory = _categoryTestFixture.GetValidCategory();

        // Act
        var category = new Category(validateCategory.Name, validateCategory.Description, false);
        category.Activate();

        //Assert
        category.IsActive.Should().BeTrue();
    }

    [Fact(DisplayName = nameof(Deactivate))]
    [Trait("Domain", "Category - Agregates")]
    public void Deactivate()
    {
        //Arrange
        var validateCategory = _categoryTestFixture.GetValidCategory();

        // Act
        var category = new Category(validateCategory.Name, validateCategory.Description, true);
        category.Deactivate();

        //Assert
        category.IsActive.Should().BeFalse();
    }

    [Fact(DisplayName = nameof(Update))]
    [Trait("Domain", "Category - Agregates")]
    public void Update()
    {
        var category = _categoryTestFixture.GetValidCategory();
        var categoryWithNewValues = _categoryTestFixture.GetValidCategory();

        category.Update(categoryWithNewValues.Name, categoryWithNewValues.Description);

        category.Name.Should().Be(categoryWithNewValues.Name);
        category.Description.Should().Be(categoryWithNewValues.Description);
    }

    [Fact(DisplayName = nameof(UpdateOnlyName))]
    [Trait("Domain", "Category - Agregates")]
    public void UpdateOnlyName()
    {
        var category = _categoryTestFixture.GetValidCategory();
        var newName = _categoryTestFixture.GetValidCategoryName();
        var currentDescription = category.Description;

        category.Update(newName);

        category.Name.Should().Be(newName);
        category.Description.Should().Be(currentDescription);
    }

    [Theory(DisplayName = nameof(UpadateErrorWhenNameIsEmpty))]
    [Trait("Domain", "Category - Agregates")]
    [InlineData("")]
    [InlineData(null)]
    [InlineData(" ")]
    public void UpadateErrorWhenNameIsEmpty(string? name)
    {
        // Arrange
        var category = _categoryTestFixture.GetValidCategory();

        Action action =
            () => category.Update(name!);

        //Act
        action.Should().Throw<DomainException>().WithMessage("Name should not be empty or null.");
    }

    [Theory(DisplayName = nameof(UpadateErrorWhenNameIsLessThan3Characters))]
    [Trait("Domain", "Category - Agregates")]
    [MemberData(nameof(GetNamesWithLessThan3Caharacters), parameters: 10)]
    public void UpadateErrorWhenNameIsLessThan3Characters(string invalidName)
    {
        // Arrange
        var category = _categoryTestFixture.GetValidCategory();
        Action action =
            () => category.Update(invalidName);

        //Act
        action.Should().Throw<DomainException>().WithMessage("Name should be at leats 3 characters.");
    }

    public static IEnumerable<object[]> GetNamesWithLessThan3Caharacters(int numberOfTests = 6)
    {
        var fixture = new CategoryTestFixture();

        for (int i = 0; i < numberOfTests; i++)
        {
            var isOdd = i % 2 == 1;

            yield return new object[] { fixture.GetValidCategoryName()[..(isOdd ? 1 : 2)] };
        }
    }

    [Fact(DisplayName = nameof(UpadateErrorWhenNameIsGreaterThan255Characters))]
    [Trait("Domain", "Category - Agregates")]
    public void UpadateErrorWhenNameIsGreaterThan255Characters()
    {
        // Arrange
        var category = _categoryTestFixture.GetValidCategory();
        var invalidName = _categoryTestFixture.Faker.Lorem.Letter(256);
        Action action =
            () => category.Update(invalidName);

        //Act
        action.Should().Throw<DomainException>().WithMessage("Name should be less or equal 255 characters long.");
    }

    [Fact(DisplayName = nameof(UpadateErrorWhenDescripionIsGreaterThan10000Characters))]
    [Trait("Domain", "Category - Agregates")]
    public void UpadateErrorWhenDescripionIsGreaterThan10000Characters()
    {
        // Arrange
        var category = _categoryTestFixture.GetValidCategory();

        var invalidDescription = _categoryTestFixture.Faker.Commerce.ProductDescription();

        while (invalidDescription.Length <= 10000)
            invalidDescription = $"{invalidDescription} {_categoryTestFixture.Faker.Commerce.ProductDescription()}";

        Action action =
            () => category.Update("Category Name", invalidDescription);

        //Act
        action.Should().Throw<DomainException>().WithMessage("Description should be less or equal 10000 characters long.");
    }
}
