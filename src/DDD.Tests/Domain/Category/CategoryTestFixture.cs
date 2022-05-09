using DDD.Catalog.Domain;
using DDD.Tests.Common;
using Xunit;

namespace DDD.Tests.Domain;
public class CategoryTestFixture :  BasicFixture
{
    public CategoryTestFixture() : base() { }

    public string GetValidCategoryName()
    {
        var categoryName = string.Empty;

        while (categoryName.Length < 3)
            categoryName = Faker.Commerce.Categories(1)[0];
        if (categoryName.Length > 255)
            categoryName = categoryName[..255];

        return categoryName;
    }

    public string GetValidCategoryDescription()
    {
        var categoryDescrption = Faker.Commerce.ProductDescription();

        if (categoryDescrption.Length > 10000)
            categoryDescrption = categoryDescrption[..10000];

        return categoryDescrption;
    }

    public Category GetValidCategory()
        => new(GetValidCategoryName(), GetValidCategoryDescription());
}

[CollectionDefinition(nameof(CategoryTestFixture))]
public class CategoryTestFixtureCollection : ICollectionFixture<CategoryTestFixture>
{
}
