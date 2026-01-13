using DragonFruit2.Validators;

namespace DragonFruit2;

public class Result<TArgs>
    where TArgs : IArgs<TArgs>
{
    public static Result<TArgs> CreateFailure(IEnumerable<ValidationFailure> failures)
    {
        var result = new Result<TArgs>();
        result.ValidationFailures.AddRange(failures);
        return result;
    }

    public readonly List<ValidationFailure> ValidationFailures = [];
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
