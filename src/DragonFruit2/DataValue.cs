namespace DragonFruit2;

public record class DataValue<TValue> : IDataValue
{
    // TODO: Replace `argsType` with `key`
    public static DataValue<TValue> Create(string name, Type argsType)
        => new(name, argsType);

    private DataValue(string name, Type argsType)
    {
        Name = name;
        ArgsType = argsType;
    }

    public string Name { get; }
    public Type ArgsType { get; }

    public TValue? Value { get; private set; }
    public DataProvider? SetBy { get; private set; }

    public bool IsSet => SetBy is not null;

    public void SetValue(TValue value, DataProvider setBy)
    {
        SetBy = setBy;
        Value = value;
    }
}
