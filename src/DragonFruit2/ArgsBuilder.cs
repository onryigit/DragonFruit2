using DragonFruit2.Validators;
using System.CommandLine;

namespace DragonFruit2;

/// <summary>
/// This type provides a common type to store ArgBuilders in the cache
/// </summary>
public abstract class ArgsBuilder
{

}

/// <summary>
/// 
/// </summary>
/// <typeparam name="TRootArgs"></typeparam>
/// <typeparam name="TRootArgs"></typeparam>
public abstract class ArgsBuilder<TRootArgs> : ArgsBuilder
   where TRootArgs : class, IArgs<TRootArgs>
{

    public abstract void Initialize(Builder<TRootArgs> builder);
    public abstract Command InitializeCli(Builder<TRootArgs> builder, CliDataProvider<TRootArgs>? cliDataProvider);
    protected abstract TRootArgs CreateInstance(Builder<TRootArgs> builder);
    protected abstract IEnumerable<ValidationFailure> CheckRequiredValues(Builder<TRootArgs> builder);

    public Builder<TRootArgs>? Builder
    {
        get;
        set
        {
            if (field is not null) throw new InvalidOperationException("The Builder only be set once");
        }
    }



    public Result<TRootArgs> CreateArgs(Builder<TRootArgs> builder, IEnumerable<ValidationFailure>? existingFailures)
    {
        existingFailures ??= Enumerable.Empty<ValidationFailure>();
        var currentFailures = existingFailures.Concat(CheckRequiredValues(builder));

        TRootArgs? args = null;
        if (!currentFailures.Any(x => x.Severity == DiagnosticSeverity.Error))
        {
            args = CreateInstance(builder);
        }

        var result = new Result<TRootArgs>(currentFailures, args);

        if (result.Args is not null)
        {
            result.AddFailures(result.Args.Validate());
        }

        return result;
    }

    protected ValidationFailure? CheckRequiredValue<TValue>(string valueName, DataValue<TValue>? dataValue)
    {
        if (dataValue is null || !dataValue.IsSet)
        {
            var message = $"The value for {valueName} is required but was not provided.";
            var idString = $"{Validator.ToValidationIdString(ValidationId.Required)};";
            return new ValidationFailure<TValue>(idString, message, valueName, DiagnosticSeverity.Error, dataValue.Value);
        }
        return null;
    }
}
