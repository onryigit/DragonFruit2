using DragonFruit2.Validators;

namespace DragonFruit2;

public abstract partial class Args<TArgs>
    where TArgs : Args<TArgs>
{
    //internal static DataBuilder<TArgs> GetDataBuilder()
    //{

    //}

    public abstract IEnumerable<ValidationFailure> Validate();



    //protected abstract TArgs CreateInstance(Builder<TArgs> builder);



}
