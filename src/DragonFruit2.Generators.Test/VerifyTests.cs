using DiffEngine;
using DragonFruit2.GeneratorSupport;
using Xunit;

namespace DragonFruit2.Generators.Test;

public class VerifyTests
{
    private readonly VerifySettings _verifySettings = new();
    public VerifyTests()
    {
        _verifySettings.UseDirectory("Snapshots");
        // DiffTools.UseOrder(DiffTool.MsWordDiff);
    }

    [Fact]
    public Task Run() =>
        VerifyChecks.Run();

    [Theory]
    [ClassData(typeof(CommandInfoTheoryData))]
    public Task CommandInfo(string desc, string argsSource, string consoleSource, CommandInfo expected, string _)
    {
        var compilation = TestHelpers.GetCompilation(argsSource, consoleSource);
        var programTree = compilation.SyntaxTrees.Last();
        var invocations = TestHelpers.GetParseArgsInvocations(programTree);
        var actual = DragonFruit2Builder.GetRootCommandInfoFromInvocation(invocations.Single(), compilation.GetSemanticModel(programTree));
        return Verify(actual, _verifySettings).UseParameters(desc);
    }

    [Theory]
    [ClassData(typeof(CommandInfoTheoryData))]
    public Task CommandOutput(string desc, string _, string __, CommandInfo commandInfo, string expected)
    {
        var actual = DragonFruit2Builder.GetSourceForCommandInfo(commandInfo);

        return Verify(actual, _verifySettings).UseParameters(desc);
    }
}
