using System.Xml.Schema;

namespace DragonFruit2.Validators;
//

public record class ValidationFailure(string Id, string Message, string ValueName, DiagnosticSeverity Severity) { }

public record class ValidationFailure<TValue>(string Id, string Message, string ValueName, DiagnosticSeverity Severity, TValue Value )
    : ValidationFailure(Id, Message, ValueName, Severity)
{ }