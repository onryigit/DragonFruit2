namespace DragonFruit2;

public abstract class ValidatorAttribute :Attribute
{
    public ValidatorAttribute(string validatorName)
    {
        ValidatorName = validatorName;
    }
    public string ValidatorName { get; set; }
}