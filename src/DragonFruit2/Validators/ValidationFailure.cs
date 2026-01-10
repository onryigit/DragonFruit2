namespace DragonFruit2.Validators;
//

public record class ValidationFailure(string Id, string Message, string Name) { }

public record class ValidationFailure<TValue>(string Id, string Message, string Name, TValue Value)
    : ValidationFailure(Id, Message, Name)
{ }