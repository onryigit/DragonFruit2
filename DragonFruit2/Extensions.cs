using System.CommandLine;

namespace DragonFruit2;

public static class CliExtensions
{
    extension(Command command)
    {
        public Command AddRange(IEnumerable<Option> options)
        {
            foreach (var option in options)
            {
                command.Add(option);
            }
            return command;
        }
        public Command AddRange(IEnumerable<Argument> arguments)
        {
            foreach (var argument in arguments)
            {  
                command.Add(argument);
            }
            return command;
        }
        public Command AddRange(IEnumerable<Command> commands)
        {
            foreach (var childCommand in commands)
            {
                command.Add(childCommand);
            }
            return command;
        }

    }
}
