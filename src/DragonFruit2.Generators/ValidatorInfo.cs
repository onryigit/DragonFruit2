
namespace DragonFruit2.Generators;

public class ValidatorInfo
{
    public required string Name { get; init; }
    public required string FullTypeName { get; init; }
    public object[] Values { get; init; } = [];
    public List<string> ConstructorArguments { get; internal set; } = [];
    public Dictionary<string, string> NamedArguments { get; internal set; } = new Dictionary<string, string>();
}
