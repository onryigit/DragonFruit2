using DragonFruit2;

namespace DragonFruit2;

public abstract class DataValues
{ 
}

public abstract class DataValues<TRootArgs> : DataValues
    where TRootArgs : class, IArgs<TRootArgs>
{
    public abstract void SetDataValues(DataProvider<TRootArgs> dataProvoder);
}
