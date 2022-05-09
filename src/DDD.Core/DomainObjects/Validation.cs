using System.Text.RegularExpressions;

namespace DDD.Core.DomainObjects;
 public class Validation
{
    public static void NotNull(object? target, string fieldName)
    {
        if (target is null)
            throw new DomainException($"{fieldName} should be not be null.");
    }

    public static void NotNullOrEmpty(string? target, string fieldName)
    {
        if (String.IsNullOrWhiteSpace(target))
            throw new DomainException($"{fieldName} should not be empty or null.");
    }

    public static void MinLenght(string target, int minLength, string fieldName)
    {
        if (target.Length < minLength)
            throw new DomainException($"{fieldName} should be at leats {minLength} characters.");
    }

    public static void MaxLenght(string target, int maxLength, string fieldName)
    {
        if (target.Length > maxLength)
            throw new DomainException($"{fieldName} should be less or equal {maxLength} characters long.");
    }

    public static void Equal(object object1, object object2, string fieldName)
    {
        if (object1.Equals(object2))
        {
            throw new DomainException($"{fieldName} should not be equal.");
        }
    }

    public static void Empty(string value, string fieldName)
    {
        if (value == null || value.Trim().Length == 0)
        {
            throw new DomainException($"{fieldName} should not be empty.");
        }
    }
}
