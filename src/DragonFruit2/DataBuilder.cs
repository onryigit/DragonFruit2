using DragonFruit2.Validators;

namespace DragonFruit2;

public abstract class DataBuilder<TArgs> where TArgs : Args<TArgs>, IArgs<TArgs>
{
    protected CliDataProvider<TArgs> GetCliDataProvider(Builder<TArgs> builder)
    {
        var cliDataProvider = builder.DataProviders.OfType<CliDataProvider<TArgs>>().FirstOrDefault();
        if (cliDataProvider is null)
        {
            cliDataProvider = new CliDataProvider<TArgs>();
            builder.DataProviders.Add(cliDataProvider);
        }
        return cliDataProvider;
    }
    public virtual void Initialize(Builder<TArgs> builder)
    {
        // No-op by default
    }

    public DataValues<TArgs> Create(Builder<TArgs> builder)
    {
        var dataValues = new DataValues<TArgs>();
        dataValues.ValidationFailures.AddRange(CheckRequiredValues(builder));
        if (dataValues.IsValid)
        {
            CreateInstance(builder);
        }

        if (dataValues.Args is not null)
        {
            dataValues.ValidationFailures.AddRange(dataValues.Args.Validate());
        }

        return dataValues;
    }

    protected abstract TArgs CreateInstance(Builder<TArgs> builder);
    protected abstract IEnumerable<ValidationFailure> CheckRequiredValues(Builder<TArgs> builder);

}
