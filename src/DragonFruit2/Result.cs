using DragonFruit2.Validators;

namespace DragonFruit2;

public class Result<TArgs>
    where TArgs : IArgs<TArgs>
{
    public List<ValidationFailure> ValidationFailures = [];
    public bool IsValid => !ValidationFailures.Any();
    public TArgs? Args { get; set; }

    public void ReportErrorsToConsole()
    {
        if (ValidationFailures.Any())
        {
            Console.WriteLine("The input was not valid. Problems included:");
            foreach (ValidationFailure failure in ValidationFailures)
            {
                Console.WriteLine($"* {failure.Message}");
            }
            Console.WriteLine();
        }
    }
}
