namespace DragonFruit2;

public class DefaultDataProvider<TRootArgs> : DataProvider
     where TRootArgs : class, IArgs<TRootArgs>

{
    public DefaultDataProvider(Builder<TRootArgs> builder)
    {
    }

    private readonly Dictionary<(Type argsType, string propertyName), object> defaultValues = new();

    public override bool TryGetValue<TValue>((Type argsType, string propertyName) key, DataValue<TValue> dataValue)
    {
        if (defaultValues.TryGetValue(key, out var value))
        {
            if (value is TValue retrievedValue)
            {
                dataValue.SetValue(retrievedValue, this);
                return true;
            }
            throw new InvalidOperationException("Issue with the default values lookup.");
        }
        return false;
    }

    public void RegisterDefault<TValue>(Type argsType, string propertyName, TValue value)
    {
        if (value is not null && !value.Equals(default(TValue)))
        {
            defaultValues[(argsType, propertyName)] = value;
        }
    }
}
