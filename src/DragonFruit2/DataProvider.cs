namespace DragonFruit2;

public abstract class DataProvider
{
    public abstract bool TryGetValue<TValue>((Type argsType, string propertyName) key,  out DataValue<TValue>? value);
}
