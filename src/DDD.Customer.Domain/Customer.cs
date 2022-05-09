using DDD.Core.DomainObjects;
using DDD.Core.DomainObjects.ValueObjects;

namespace DDD.Customer.Domain;
public class Customer : Entity, IAggregateRoot
{
    public Customer(string name, string email, string cpf, bool isActive = true)
    {
        Name = name;
        Email = new Email(email);
        Cpf = new Cpf(cpf);
        IsActive = isActive;
        CreatedAt = DateTime.Now;

        Validate();
    }

    public string Name { get; private set; }
    public Email Email { get; private set; }
    public Cpf Cpf { get; private set; }
    public bool IsActive { get; private set; }
    public DateTime CreatedAt { get; set; }
    public Address? Address { get; private set; }

    public void AssignAddress(Address address)
    {
        Address = address;
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
    }
}
