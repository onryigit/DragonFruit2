using DragonFruit2.Validators;

namespace DragonFruit2;

public abstract class Args<TArgs>
    where TArgs : Args<TArgs>
{
    public abstract IEnumerable<ValidationFailure> Validate();
}
