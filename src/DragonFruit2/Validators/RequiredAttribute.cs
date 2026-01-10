namespace DragonFruit2.Validators;


// TODO: Add analyzer to ensure the CompareWith type in the attribute matches the property type
[AttributeUsage(AttributeTargets.Property, Inherited = true, AllowMultiple = false)]
public sealed class RequiredAttribute : ValidatorAttribute
{

    // This is a positional argument
    public RequiredAttribute(object compareWith)
        : base(nameof(GreaterThanValidator<>))
    { }

}
