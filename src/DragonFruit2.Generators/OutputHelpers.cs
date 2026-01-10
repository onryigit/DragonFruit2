namespace DragonFruit2.Generators;

public class OutputHelpers
{
    public static string GetLocalSymbolName(string name)
    {
        return $"{char.ToLower(name[0])}{name.Substring(1)}";
    }
}
