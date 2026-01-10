namespace DragonFruit2.Validators;

/// <summary>
/// Provides a base class for implementing value validators with a unique identifier and descriptive metadata.
/// </summary>
/// <remarks>
/// The non-generic class makes it easier to supply help information on validation.
/// </remarks>
public abstract class Validator
{
    protected const string DragonFruitValidationPrefix = "DR";

    protected Validator(string id, string valueName)
    {
        Id = id;
        ValueName = valueName;
    }

    public string Id { get; init; }
    public string ValueName { get; }
    public abstract string Description { get; }
}

public abstract class Validator<TValue> : Validator
{
    protected Validator(string id, string valueName)
        : base(id, valueName)
    {  }

    public abstract IEnumerable<ValidationFailure<TValue>> Validate( TValue value);
}

public enum ValidationId
{
    GreaterThan = 1,
    GreaterThanOrEqual = 2, 
    LessThan = 3,
    LessThanOrEqual = 4
}