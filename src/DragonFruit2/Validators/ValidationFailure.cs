namespace DragonFruit2.Validators;

public record class ValidationFailure { }

public record class ValidationFailure<TValue>(string Id, string Message, string name, TValue Value)
    : ValidationFailure
{ }