using System.Drawing;
using DragonFruit2.Validators;

namespace SampleConsoleApp;

public partial class MyArgs
{
    /// <summary>
    /// Your name
    /// </summary>
    public required string Name { get; set; }

    /// <summary
    /// Your age"
    /// </summary>
    [GreaterThan(1)]
    public int Age { get; set; } = 0;

    /// <summary>
    /// Greeting message
    /// </summary>
    public string Greeting { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the user's preferred color.
    /// </summary>
    public Color FavoriteColor { get; set; } = Color.Blue;
}
