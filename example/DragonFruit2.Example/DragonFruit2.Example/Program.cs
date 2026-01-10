using DragonFruit2;
using SampleConsoleApp;

var myArgs = Cli.ParseArgs<MyArgs>(args);

if (myArgs is not null)
{
    var greeting = myArgs.Greeting ?? "Hello";
    var drink = ", would you like some wine?";
    var noDrink = ".";
    Console.WriteLine($"{greeting} {myArgs.Name}(Age: {myArgs.Age}){(myArgs.Age >= 18 ? drink : noDrink)}");
    Console.WriteLine($"Is your favorite color {myArgs.FavoriteColor.Name}?");
}