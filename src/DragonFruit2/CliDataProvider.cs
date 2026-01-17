using DragonFruit2.Validators;
using System.CommandLine;
using System.CommandLine.Parsing;

namespace DragonFruit2;

public class CliDataProvider<TRootArgs> : DataProvider, IActiveArgsBuilderProvider<TRootArgs>
    where TRootArgs : class, IArgs<TRootArgs> 
{
    public CliDataProvider(Builder<TRootArgs> builder) 
    {
        InputArgs = builder.CommandLineArguments;
    }

    public string[]? InputArgs { get; }

    public (IEnumerable<ValidationFailure>? failures, ArgsBuilder<TRootArgs>? builder) GetActiveArgsBuilder()
    {
        ParseResult = RootCommand?.Parse(InputArgs);
        ArgsBuilderCache<TRootArgs>.ActiveArgsBuilder = null;
        var _ = ParseResult?.Invoke();
        var failures = ParseResult is not null && ParseResult.Errors.Any()
                        ? TransformErrors(ParseResult.Errors)
                        : Enumerable.Empty<ValidationFailure>();

        return (failures, ArgsBuilderCache<TRootArgs>.GetActiveArgsBuilder());
    }

    private IEnumerable<ValidationFailure>? TransformErrors(IReadOnlyList<ParseError> errors)
    {
        return errors.Select(CreateValidationFailure);

        static ValidationFailure CreateValidationFailure(ParseError error)
            => new(ValidationId.SystemCommandLine.ToValidationIdString(),
                   error.Message,
                   string.Empty,
                   DiagnosticSeverity.Error);
    }

    public Command? RootCommand
    {
        get;
        set;
    }

    public ParseResult? ParseResult
    {
        get => field ??= RootCommand?.Parse(InputArgs);
        private set;
    }

    public Dictionary<(Type argsType, string propertyName), Symbol> LookupSymbol { get; set; } = [];

    public override bool TryGetValue<TValue>((Type argsType, string propertyName) key,out DataValue<TValue>? dataValue)
    {
        if (RootCommand is null) throw new ArgumentNullException(nameof(RootCommand));
        if (InputArgs is null) throw new ArgumentNullException(nameof(InputArgs));
        ParseResult ??= RootCommand.Parse(InputArgs);

        var symbol = LookupSymbol[key];
        if (symbol is not null)
        {
            var symbolResult = ParseResult.GetResult(symbol);
            if (symbolResult is not null)
            {
                // Symbol was found, value may or may not have been provided
                dataValue = GetValue(ParseResult, symbol, this);
                return dataValue is not null;
            }
        }
        dataValue = null;
        return false;

        static DataValue<TValue>? GetValue(ParseResult parseResult, Symbol symbol, DataProvider dataProvider)
        {
            var symbolResult = parseResult.GetResult(symbol);
            if (symbolResult is null) return null;

            var resultFound = false;
            TValue? value = default;

            if (symbolResult.Tokens.Any())
            {
                // Except for Boolean switches, do not use default values, but only those specified via tokens
                resultFound = true;
                value = symbol switch
                {
                    Argument argument => parseResult.GetValue<TValue>(argument.Name),
                    Option option => parseResult.GetValue<TValue>(option.Name),
                    _ => throw new InvalidOperationException("Unsupported symbol type")
                };
            }
            else if (symbolResult is OptionResult optionResult && optionResult.Option.ValueType == typeof(bool))
            {
                // If the user specified the option, use the default value, which is provided by GetValue
                if (optionResult.IdentifierTokenCount > 0)
                {
                    resultFound = true;
                    value = parseResult.GetValue<TValue>(optionResult.Option.Name);
                }
            }

            return resultFound
                ? DataValue<TValue>.Create(value, dataProvider)
                : null;
        }
    }

    public void AddNameLookup((Type argsType, string propertyName) key, Symbol symbol)
    {
        LookupSymbol[key] = symbol;
    }

}
