namespace DragonFruit2;

public record class DataValue<T> : IDataValue
{
    public static DataValue<T> Create(T? value, DataProvider setBy)
        => new(value, setBy);

    private DataValue(T? value, DataProvider setBy)
    {
        Value = value;
        SetBy = setBy;
    }

    public T? Value { get; }

    public DataProvider SetBy { get; }

    public bool IsSet => SetBy is not null;
}
