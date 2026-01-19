namespace DragonFruit2;

public abstract class DataProvider
{
    public abstract bool TryGetValue<TValue>((Type argsType, string propertyName) key, DataValue<TValue> value);

    public void SetDataValue<TValue>((Type argsType, string propertyName) key, DataValue<TValue> dataValue)
    {
        if (TryGetValue(key, dataValue))
        {
            return;
        }
    }
}
