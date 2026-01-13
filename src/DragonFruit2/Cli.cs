namespace DragonFruit2;

public static class Cli
{

    /// <summary>
    /// </summary>
    /// <typeparam name="TRootArgs">The type containing the CLI definition</typeparam>
    /// <param name="args">Optionaly pass the commandline args</param>
    /// <returns></returns>
    public static DataValues<TRootArgs> ParseArgs<TRootArgs>(string[]? args = null)
        where TRootArgs : Args<TRootArgs>
    {
        throw new InvalidOperationException("This method is a stub for source generation. If you see this error, there is probably a problem with source generation.");
        //args ??= Environment.GetCommandLineArgs();

        //var argsDataValues = new Builder<TRootArgs>().ParseArgs(args);


        //return argsDataValues;

    }

    /// <summary>
    /// </summary>
    /// <typeparam name="TRootArgs">The type containing the CLI definition</typeparam>
    /// <param name="args">Optionaly pass the commandline args</param>
    /// <returns></returns>
    public static DataValues<TRootArgs> ParseArgs<TRootArgs>(ArgsBuilder<TRootArgs> rootArgsBuilder, string[]? args = null)
        where TRootArgs : Args<TRootArgs>
    {
        args ??= Environment.GetCommandLineArgs();

        var argsDataValues = new Builder<TRootArgs>().ParseArgs(rootArgsBuilder, args);


        return argsDataValues;

    }
}
