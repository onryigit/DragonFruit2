namespace DragonFruit2.Validators;

public enum ValidationId
{
    GreaterThan = 1,
    GreaterThanOrEqual = 2, 
    LessThan = 3,
    LessThanOrEqual = 4,
    Required = 5
}

public static class ValidationIdExtensions
{
    private static readonly string DragonFruitValidationPrefix = "DR";

    extension(ValidationId id)
    {
        public string ToValidationIdString()
        {
            return $"{DragonFruitValidationPrefix}{(int)id:000}";
        }
    }
}