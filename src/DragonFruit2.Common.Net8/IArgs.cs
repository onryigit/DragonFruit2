namespace DragonFruit2.Common;

public interface IArgs<TArgs, TData>
    where TArgs :IArgs<TArgs, TData>
{
    public abstract static TData Initialize<TData>();
    public abstract static TArgs Create(Runner<TArgs> runner);

}
