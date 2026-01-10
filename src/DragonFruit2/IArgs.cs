namespace DragonFruit2;

public interface IArgs<TArgs>
    where TArgs : Args<TArgs>, IArgs<TArgs>
{
    public static abstract DataBuilder<TArgs> GetDataBuilder(Builder<TArgs> builder);

}
