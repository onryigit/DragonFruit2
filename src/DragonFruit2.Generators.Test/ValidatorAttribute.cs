namespace DragonFruit2.Generators.Test;

[AttributeUsage(AttributeTargets.Property, Inherited = true, AllowMultiple = false)]
public class ValidatorAttribute : Attribute
{
    public ValidatorAttribute(string validatorName)
    {

        ValidatorName = validatorName;
    }
    public string ValidatorName { get; set; }
}


