using SampleConsoleApp;

namespace SimpleConsoleApp;

/// <summary>
/// Test that run generation as part of build, and these tests are on that output
/// </summary>
/// <remarks>
/// The purpose of these tests is to ensure that the generated code runs correctly. 
/// The code is created _automatically on build_ via normal generation with a _project_ reference
/// to the generator. We might later consider the value of using a package reference.
/// <br/>
/// The generated code (that you will probably need to troubleshoot tests) is in 
/// "Dependencies/Analyzers/DragonFruit2.Generators/DragonFruit2.Generator.DragonFruit2Generator"
/// node of the solution exporer of this project in Visual Studio.
/// <br/>
/// Note that this does not call the 
/// </remarks>
public class IntegrationTests
{
    private TextWriter originalOutput = Console.Out;

    private void SetConsoleOut()
    {
        // Create a StringWriter to capture output
        var stringWriter = new StringWriter();
        // Save the original Console.Out

        // Redirect Console.Out to the StringWriter
        Console.SetOut(stringWriter);
    }

    private void ResetConsoleOut()
    {
        // Create a StringWriter to capture output

        // Restore the original Console.Out in the finally block
        Console.SetOut(originalOutput);
    }

    [Theory]
    [ClassData(typeof(ParseArgsTheoryData))]
    public void String_options_are_retrieved(string cliInput, string consoleOutput)
    {
        try
        {
            SetConsoleOut();
        }
        finally
        {
            ResetConsoleOut();
        }
    }

    [Theory]
    [ClassData(typeof(ParseArgsTheoryData))]
    public void Int_options_are_retrieved(string cliInput, string consoleOutput)
    {
        try
        {
            SetConsoleOut();
        }
        finally
        {
            ResetConsoleOut();
        }
    }

    [Theory]
    [ClassData(typeof(ParseArgsTheoryData))]
    public void Default_values_are_applied(string cliInput, string consoleOutput)
    {
        try
        {
            SetConsoleOut();
        }
        finally
        {
            ResetConsoleOut();
        }
    }

    //[Theory]
    //[ClassData(typeof(ExecuteOutput))]
    //public void TestGeneratedCodeViaHappyPaths(string cliInput, string consoleOutput)
    //{
    //    try
    //    {
    //        SetConsoleOut();
    //        var result = Cli.ParseArgs<MyArgs>();
    //    }
    //    finally
    //    {
    //        ResetConsoleOut();
    //    }
    //}
}
