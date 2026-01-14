using DragonFruit2.Validators;

namespace DragonFruit2;

public class Builder<TRootArgs>
    where TRootArgs : class, IArgs<TRootArgs>
{
    public Builder()
    {
        AddDataProvider(new CliDataProvider<TRootArgs>());
    }

    public List<DataProvider> DataProviders { get; } = [];

    public void AddDataProvider(DataProvider provider, int position = int.MaxValue)
    {
        if (position < int.MaxValue)
        {
            DataProviders.Insert(position, provider);
        }
        else
        {
            DataProviders.Add(provider);
        }
    }

    public DataValue<T> GetDataValue<T>(string key, params object[] alternateKeys)
    {
        foreach (var dataProvider in DataProviders)
        {
            if (dataProvider.TryGetValue<T>(key, alternateKeys, out var dataValue))
            {
                return dataValue;
            }
        }
        return DataValue<T>.CreateEmpty();
    }


    public Result<TRootArgs> ParseArgs(ArgsBuilder<TRootArgs> argsBuilder, string[] args)
    {
        //var argsBuilder = ArgsBuilderCache<TRootArgs>.GetArgsBuilder<TRootArgs>();
        // The entire CLI tree is built from the TRootArgs
        var rootCommand = argsBuilder.InitializeCli(this);

        var cliDataProvider = DataProviders.OfType<CliDataProvider<TRootArgs>>().FirstOrDefault()
            ?? throw new InvalidOperationException("Internal error: CliDataProvider not found");
        cliDataProvider.RootCommand = rootCommand;
        // Once you set the InputArgs, the provider can start parsing
        var (failures, activeArgsBuilder) = cliDataProvider.GetActiveArgsBuilder(args);

        return activeArgsBuilder is null
                    ? new Result<TRootArgs>(failures, null)
                    : activeArgsBuilder.CreateArgs(this, failures);
    }
}
