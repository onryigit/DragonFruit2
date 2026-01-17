using DragonFruit2;

namespace DragonFruit2;

public abstract class Builder<TRootArgs>
    where TRootArgs : class, IArgs<TRootArgs>
{
    protected abstract ArgsBuilder<TRootArgs> GetRootArgsBuilder();

    protected Builder(string[] commandLineArguments, DragonFruit2Configuration? configuration = null)
    {
        CommandLineArguments = commandLineArguments;
        AddDataProvider(new CliDataProvider<TRootArgs>(this));
        AddDataProvider(new DefaultDataProvider<TRootArgs>(this));
        Configuration = configuration;
    }

    public string[] CommandLineArguments { get; }

    public List<DataProvider> DataProviders { get; } = [];
    public DragonFruit2Configuration? Configuration { get; }

    public TDataProvider GetDataProvider<TDataProvider>()
            where TDataProvider : DataProvider
        => DataProviders.OfType<TDataProvider>().FirstOrDefault();

    public void AddDataProvider(DataProvider provider, int position = int.MaxValue)
    {
        // TODO: Should we protect against multiple entries of the same provider? The same provider type? (might be scenarios for that) Have an "allow multiples" trait on the provider? (How would we do that in Framework?) Have each provider build a key that could differentiate?
        if (position < int.MaxValue)
        {
            DataProviders.Insert(position, provider);
        }
        else
        {
            DataProviders.Add(provider);
        }
    }

    public DataValue<T>? GetDataValue<T>((Type argsType, string propertyName) key)
    {
        foreach (var dataProvider in DataProviders)
        {
            if (dataProvider.TryGetValue<T>(key, out var dataValue))
            {
                return dataValue;
            }
        }
        return null;
    }

    public Result<TRootArgs> ParseArgs(string[] args)
    {
        GetRootArgsBuilder().Initialize(this);

        var cliDataProvider = DataProviders.OfType<IActiveArgsBuilderProvider<TRootArgs>>().FirstOrDefault()
            ?? throw new InvalidOperationException("Internal error: CliDataProvider not found");
        // Once you set the InputArgs, the provider can start parsing
        var (failures, activeArgsBuilder) = cliDataProvider.GetActiveArgsBuilder();

        return activeArgsBuilder is null
                    ? new Result<TRootArgs>(failures, null)
                    : activeArgsBuilder.CreateArgs(this, failures);
    }
}

internal class Builder<TRootArgs, TRootArgsBuilder> : Builder<TRootArgs>
        where TRootArgs : class, IArgs<TRootArgs>
        where TRootArgsBuilder : ArgsBuilder<TRootArgs>, new()
{
    private TRootArgsBuilder argsBuilder;

    public Builder(string[] args)
        : base(args)
    {
        argsBuilder = new TRootArgsBuilder()
        {
            Builder = this
        };
    }

    protected override ArgsBuilder<TRootArgs> GetRootArgsBuilder()
    {
        return argsBuilder;
    }


    public Builder(string[] commandLineArguments, DragonFruit2Configuration? configuration = null)
        : base(commandLineArguments, configuration)
    {
    }
}


