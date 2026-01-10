namespace DragonFruit2.Generators.Test;

public sealed class ValidatorTestAttributeOneCtorParam : ValidatorAttribute
{

    // This is a positional argument
    public ValidatorTestAttributeOneCtorParam(object compareWith)
        : base("ValidatorTest")
    {
        CompareWith = compareWith;
    }

    public object CompareWith { get; }


    public int AnotherValue { get; set; }
}

public sealed class ValidatorTestAttributeOneNamedParam : ValidatorAttribute
{
    public ValidatorTestAttributeOneNamedParam()
        : base("ValidatorTest")
    {}


    public int AnotherValue { get; set; }
}

public sealed class ValidatorTestAttributeMulitpleMixedParams : ValidatorAttribute
{
    public ValidatorTestAttributeMulitpleMixedParams(object compareWith, object second)
        : base("ValidatorTest")
    { }


    public int AnotherValue { get; set; }

    public int StillAnother { get; set; }
}


