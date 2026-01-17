using DragonFruit2;
using SampleConsoleApp;

public class Cli 
{
    public static Result<MyArgs> ParseArgs<TRootArgs>(string[]? args = null)
        where TRootArgs : IArgs<TRootArgs>
    {     
        return DragonFruit2.Cli.ParseArgs<MyArgs, MyArgs.MyArgsBuilder>( args);
    }
}
