using DDD.Tests.Common;
using System;
using Xunit;

namespace DDD.Tests.Domain.Product;
public class ProductTestFixture : BasicFixture
{
    public ProductTestFixture() : base() { }

    public string GetValidProductName()
    {
        var productName = string.Empty;

        while (productName.Length < 3)
            productName = Faker.Commerce.ProductName();
        if (productName.Length > 255)
            productName = productName[..255];

        return productName;
    }

    public string GetValidProductDescription()
    {
        var productDescrption = Faker.Commerce.ProductDescription();

        if (productDescrption.Length > 10000)
            productDescrption = productDescrption[..10000];

        return productDescrption;
    }

    public decimal GetValidProductPrice()
    {
        var productPrice = Convert.ToDecimal(Faker.Commerce.Price());

        return productPrice;
    }

    public Guid GetValidCategoryId()
    {
        return Guid.NewGuid();
    }

    public Catalog.Domain.Product GetValidProduct()
    {
        return new(GetValidCategoryId(), GetValidProductName(), GetValidProductDescription(), GetValidProductPrice());
    }
}

[CollectionDefinition(nameof(ProductTestFixture))]
public class ProductTestFixtureCollection : ICollectionFixture<ProductTestFixture>
{
}
