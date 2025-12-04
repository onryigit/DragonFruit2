using DragonFruit2.Common.Net8;
using System.CommandLine;
using System.Data;

namespace DragonFruit2;

public abstract class CliArgs : Args
{
    //protected static CliDataProvider GetCliDataProvider()
    //{
    //    var cliProvider = DataProviders.OfType<CliDataProvider>().FirstOrDefault();
    //    if (cliProvider is null)
    //    {
    //        cliProvider = new CliDataProvider();
    //        AddDataProvider(cliProvider, 0);
    //    }

    //    return cliProvider;
    //}

    //public static CliDataProvider SetParseResult(ParseResult parseResult)
    //{
    //    var cliProvider = GetCliDataProvider();
    //    cliProvider.ParseResult = parseResult;
    //    return cliProvider;
    //}

}
