using DragonFruit2.Common;

namespace DragonFruit2;

public static class Cli
{
 
    /// <summary>
    /// </summary>
    /// <typeparam name="TArgs">The type containing the CLI definition</typeparam>
    /// <param name="args">Optionaly pass the commandline args</param>
    /// <returns></returns>
    public static TArgs? ParseArgs<TArgs>(string[]? args = null)
        where TArgs : ICliArgs<TArgs>
    {
        args ??= Environment.GetCommandLineArgs();
        var command = TArgs.CreateCli();
        command.SetAction(parseResult =>
        {
            return 0;
        });

        var parseResult = command.Parse(args);
        var returnCode = parseResult.Invoke();
        if (returnCode != 0)
        {
            return default;
        }
        TArgs? argsObject = TArgs.Create(parseResult);
        return argsObject;

    }
}
