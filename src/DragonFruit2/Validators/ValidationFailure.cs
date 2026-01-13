namespace DragonFruit2.Validators;
//

public record class ValidationFailure(string Id, string Message, string ValueName) { }

public record class ValidationFailure<TValue>(string Id, string Message, string ValueName, TValue Value)
    : ValidationFailure(Id, Message, ValueName)
{ }