using DDD.Core.DomainObjects;

namespace DDD.Catalog.Domain;
public class Category : Entity
{
    public Category(string name, string description, bool isActive = true)
    {
        Name = name;
        Description = description;
        IsActive = isActive;
        CreatedAt = DateTime.Now;

        Validate();
    }

    public string Name { get; private set; }
    public string Description { get; private set; }
    public bool IsActive { get; private set; }
    public DateTime CreatedAt { get; set; }
    public ICollection<Product>? Products { get; set; }


    public void Update(string name, string? description = null)
    {
        Name = name;
        Description = description ?? Description;

        Validate();
    }

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

    public void Validate()
    {
        Validation.NotNullOrEmpty(Name, nameof(Name));
        Validation.MinLenght(Name, 3, nameof(Name));
        Validation.MaxLenght(Name, 255, nameof(Name));

        Validation.NotNull(Description, nameof(Description));
        Validation.MaxLenght(Description, 10000, nameof(Description));
    }
}
