using System.CommandLine;
using System.IO;

namespace DragonFruit2;

public static class CLiBuilder
{
 
    /// <summary>
    /// </summary>
    /// <typeparam name="T">The type containing the CLI definition</typeparam>
    /// <param name="args">Optionaly pass the commandline args</param>
    /// <returns></returns>
    public static T? ParseArgs<T>(string[]? args = null)
        where T : IArgs<T>
    {
        args ??= Environment.GetCommandLineArgs();
        var command = T.CreateCli();
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
        T? argsObject = T.Create(parseResult);
        return argsObject;

    }
}
