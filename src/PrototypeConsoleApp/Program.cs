using DragonFruit2;
using SampleConsoleApp;

var myArgsDataValues = Cli.ParseArgs<MyArgs>(args);

if (myArgsDataValues.IsValid)
{
    var myArgs = myArgsDataValues.Args!; // Safe to use '!' because IsValid is true
    var drink = ", would you like some wine?.";
    var noDrink = ".";
    Console.WriteLine($"{myArgs.Greeting} {myArgs.Name}{(myArgs.Age >= 18 ? drink : noDrink)}");
}
else
{
    // Display validation errors
}
