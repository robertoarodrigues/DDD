using DDD.Core.DomainObjects;

namespace DDD.Catalog.Domain;
public class Product : Entity, IAggregateRoot
{
    public Product(Guid categoryId, string name, string description, decimal price, bool isActive = true)
    {
        CategoryId = categoryId;
        Name = name;
        Description = description;
        IsActive = isActive;
        Price = price;
        CreatedAt = DateTime.Now;

        Validate();
    }

    public Guid CategoryId { get; private set; }
    public string Name { get; private set; }
    public string Description { get; private set; }
    public bool IsActive { get; private set; }
    public decimal Price { get; private set; }
    public DateTime CreatedAt { get; set; }

    public void Activate()
    {
        IsActive = true;
        Validate();
    }
    public void Deactivate() 
    {
        IsActive = false;
        Validate();
    } 

    public void ChangeCategory(Category category)
    {
        CategoryId = category.Id;
        Validate();
    }
    public void Update(string name, string? description = null, decimal? price = null)
    {
        Name = name;
        Description = description ?? Description;
        Price = price ?? Price;

        Validate();
    }

    public void Validate()
    {
        Validation.NotNullOrEmpty(Name, nameof(Name));
        Validation.MinLenght(Name, 3, nameof(Name));
        Validation.MaxLenght(Name, 255, nameof(Name));

        Validation.NotNull(Description, nameof(Description));
        Validation.MaxLenght(Description, 10000, nameof(Description));
    }
}
