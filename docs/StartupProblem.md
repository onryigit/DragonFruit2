# Startup problem

There is a pernicious problem getting a toehold between the generalized code and the user's specific code. This could be solved a number of ways, but these goals restrict the options:

* Work in .NET Standard/Framework and almost certainly with user code in C# 7.1
* Not add extra work for the user
* Not requiring the user to write code that is not in the IntelliSense dropdown (don't rely on generation for basic semantics)
* Not creating an instance of the `Args` class because this could restrict important instantiation rules - such as disallowing immutability

The design is user 

* `Args` classes that have limited changes from what the user wrote
*` _ArgsName_ArgsBuilder` classes that create the CLI and build the DataValues and Args instance
* DataValues classes that allow the user to access what set the value

The problem can appear in different ways with different designs, but can be summarized as "given an Args class, find the corresponding ArgsBuilder class". This needs to occur twice:

* At CLI startup when the _root_ `ArgsBuilder` is needed to build the CLI
* Handling the `ParseResult` to create the right `Args` instance

If we solve, the first, determining the active Args for the command can be done by setting a static value from the action of the command. Not pretty, but reliably effective. The static value is specific to the Root via generic magic.

I have not yet found a mechanism for initially finding the root's `ArgsBuilder`. I have a cache that is filled via static constructors in the `ArgsBulder` classes, but these constructors are not executed because we are not accessing the type.

Some things I have tried or considered:

* An `IArgs<TRootArgs>` interface with a static method to get the data (not valid in .NET Framework)
* Module Initializer to create a cache; the cache exists in the current design (not valid in .NET Framework)
* Using overload resolution for methods and extension methods to have a non-specific `ParseArgs` method for editing and a generated one
  * This would require changing to an instance method for .NET Framework. 

Other ideas

* Creating an extension method in DragonFruit2 which is 