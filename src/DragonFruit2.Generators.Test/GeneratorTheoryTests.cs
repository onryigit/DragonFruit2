using DragonFruit2.GeneratorSupport;
using Xunit;

namespace DragonFruit2.Generators.Test;

public class GeneratorTheoryTests
{
    [Theory]
    [ClassData(typeof(CommandInfoTheoryData))]
    public async Task CreatesExpectedCommandInfo(string desc, string argsSource, string consoleSource, CommandInfo expected, string _)
    {
        var compilation = TestHelpers.GetCompilation(argsSource, consoleSource);
        var programTree = compilation.SyntaxTrees.Last();
        var invocations = TestHelpers.GetParseArgsInvocations(programTree);
        Assert.Single(invocations);

        var actual = DragonFruit2Builder.GetRootCommandInfoFromInvocation(invocations.Single(), compilation.GetSemanticModel(programTree));

        Assert.Equivalent(expected, actual);
    }

    [Theory]
    [ClassData(typeof(CommandInfoTheoryData))]
    public async Task CreatesExpectedCommandOutput(string desc, string _, string __, CommandInfo commandInfo, string expected)
    {
        var actual = DragonFruit2Builder.GetSourceForCommandInfo(commandInfo);

        Assert.Equal(expected, actual);
    }
}
