using DragonFruit2.Common.Net8;
using System.CommandLine;

namespace DragonFruit2;

public interface ICliArgs<TArgs> : IArgs<TArgs>
{
    public abstract static System.CommandLine.Command CreateCli();
    public abstract static TArgs Create(ParseResult parseResult); 
}
