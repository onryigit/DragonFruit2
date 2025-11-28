using DragonFruit2.GeneratorSupport;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DragonFruit2.Generators;

public class DragonFruit2Builder : GeneratorBuilder<CommandInfo>
{
    private static int indentSize = 4;

    /// <summary>
    /// Determines whether the specified syntax node represents an invocation of the generic method 'ParseArgs' with
    /// exactly one type argument.
    /// </summary>
    /// <param name="node">The syntax node to evaluate.</param>
    /// <returns>true if the node is an invocation of 'ParseArgs' with a single type argument; otherwise, false.</returns>
    public override bool InitialFilter(SyntaxNode node)
        => (node is InvocationExpressionSyntax inv) &&
        inv.Expression switch
        {
            MemberAccessExpressionSyntax ma when ma.Name is GenericNameSyntax gns
                => gns.Identifier.ValueText == "ParseArgs" && gns.TypeArgumentList.Arguments.Count == 1,
            GenericNameSyntax gns2
                => gns2.Identifier.ValueText == "ParseArgs" && gns2.TypeArgumentList.Arguments.Count == 1,
            _ => false,
        };

    public override CommandInfo? Transform(GeneratorSyntaxContext context)
            // We only get here for ParseArg invocations with a single generic type argument
            => context.Node switch
            {
                InvocationExpressionSyntax invocationSyntax
                       => GetRootCommandInfoFromInvocation(invocationSyntax, context.SemanticModel),
                _ => null,
            };

    public override void OutputSource(SourceProductionContext context, IEnumerable<CommandInfo> items)
    {
        foreach (var commandInfo in items)
        {
            var sourceText = GetSourceForCommandInfo(commandInfo);
            context.AddSource($"{commandInfo.Name}_ParseArgsGenerator.g.cs", sourceText);
        }
    }


    public static CommandInfo? GetRootCommandInfoFromInvocation(InvocationExpressionSyntax invocationSyntax, SemanticModel semanticModel)
    {
        var rootArgTypeArgSymbol = GetArgTypeSymbol(invocationSyntax, semanticModel);
        if (rootArgTypeArgSymbol is null)
            return null; // This occurs when the root arg type does not yet exist
        var rootCommandInfo = CreateCommandInfo(rootArgTypeArgSymbol, semanticModel);
        return rootCommandInfo;
    }

    private static CommandInfo CreateCommandInfo(INamedTypeSymbol typeSymbol, SemanticModel semanticModel)
    {

        var commandInfo = CommandInfoHelpers.CreateCommandInfo(typeSymbol);

        var props = typeSymbol.GetMembers()
                              .OfType<IPropertySymbol>()
                              .Where(p => !p.IsStatic)
                              .Select(p => CommandInfoHelpers.CreatePropInfo(p))
                              .ToList();

        // Split into argument list and options
        var argList = props.Where(p => p.IsArgument).OrderBy(p => p.Position).ToList();
        var optList = props.Where(p => !p.IsArgument).ToList();
        commandInfo.Arguments.AddRange(argList);
        commandInfo.Options.AddRange(optList);

        var compilation = semanticModel.Compilation;

        var derivedTypes = GetChildTypes(typeSymbol);
        foreach (var derivedType in derivedTypes)
        {
            var childCommandInfo = CreateCommandInfo(derivedType, semanticModel);
            commandInfo.SubCommands.Add(childCommandInfo);
        }

        return commandInfo;
    }

    private static IEnumerable<INamedTypeSymbol> GetChildTypes(INamedTypeSymbol typeSymbol)
    {
        var derivedTypes = new List<INamedTypeSymbol>();
        var nspace = typeSymbol.ContainingNamespace;
        foreach (var member in nspace.GetMembers())
        {
            if (member is INamedTypeSymbol namedTypeSymbol)
            {
                // Check if this type derives from the given typeSymbol
                if (SymbolEqualityComparer.Default.Equals(namedTypeSymbol.BaseType, typeSymbol))
                {
                    // This namedTypeSymbol derives from typeSymbol
                    derivedTypes.Add(namedTypeSymbol.BaseType);
                    break;
                }
            }
        }
        return derivedTypes;
    }

    private static INamedTypeSymbol? GetArgTypeSymbol(InvocationExpressionSyntax? invocation, SemanticModel semanticModel)
    {
        if (invocation is null) return null;

        GenericNameSyntax? genericName = invocation.Expression switch
        {
            MemberAccessExpressionSyntax ma when ma.Name is GenericNameSyntax gns
                => gns,
            GenericNameSyntax gns
                => gns,
            _ => null,
        };

        if (genericName is null) return null;

        var typeArgSyntax = genericName.TypeArgumentList.Arguments[0];

        return semanticModel.GetSymbolInfo(typeArgSyntax).Symbol as INamedTypeSymbol;
    }

    internal static string GetSourceForCommandInfo(CommandInfo commandInfo)
    {
        var indent = "";
        var description = commandInfo.Description is null
                              ? "null"
                              : $"\"{commandInfo.Description.Replace("\"", "\"\"")}\"";

        var sb = new StringBuilder();
        sb.Append(OutputFileOpen(commandInfo, description));
        sb.AppendLine();
        if (!string.IsNullOrEmpty(commandInfo.NamespaceName))
        {
            sb.AppendLine($"namespace {commandInfo.NamespaceName}");
            sb.AppendLine("{");
            indent += new string(' ', indentSize);
        }
        var classOrStruct = commandInfo.IsStruct ? "struct" : "class";
        sb.Append($$"""
                {{indent}}public partial {{classOrStruct}} {{commandInfo.Name}} : IArgs<{{commandInfo.Name}}>
                {{indent}}{
                {{indent}}    public static System.CommandLine.Command CreateCli()
                {{indent}}    {
                {{indent}}        var rootCommand = new System.CommandLine.Command("Test")
                {{indent}}            {
                {{indent}}                Description = {{description}}
                {{indent}}            };
                """);


        foreach (var option in commandInfo.Options)
        {
            GetOptionDeclaration(sb, option);
        }
        foreach (var argument in commandInfo.Arguments)
        {
            GetArgumentDeclaration(sb, argument);
        }
        foreach (var subcommand in commandInfo.SubCommands)
        {
            GetSubCommandDeclaration(sb, subcommand);
        }
        sb.AppendLine();
        sb.Append($$"""
                       {{indent}}        return rootCommand;
                       {{indent}}    }
                       """);
        sb.AppendLine();
        sb.AppendLine();
        sb.Append($$"""
                       {{indent}}    public static {{commandInfo.Name}} Create(ParseResult parseResult)
                       {{indent}}    {
                       {{indent}}        var newArgs = new {{commandInfo.Name}}()
                       {{indent}}        {
                       {{indent}}        };
                       {{indent}}        return newArgs;
                       {{indent}}    }
                       {{indent}}}

                       """);
        if (!string.IsNullOrEmpty(commandInfo.NamespaceName))
        {
            indent = indent.Substring(0, indent.Length - indentSize);
            sb.AppendLine($"{indent}}}");
        }
        return sb.ToString();
    }

    private static string OutputFileOpen(CommandInfo commandInfo, string description)
    {
        return $$"""
                       // <auto-generated />
                       using DragonFruit2;
                       using System.CommandLine;

                       """;
    }

    internal static void GetSubCommandDeclaration(StringBuilder sb, CommandInfo commandInfo)
    {
        sb.Append($$"""
                                   commandInfo.Add(new System.CommandLine.Command("Test")              
                                   {
                                       Description = { { description } }
                                   };
                       """);
        foreach (var option in commandInfo.Options)
        {
            GetOptionDeclaration(sb, option);
        }
        foreach (var argument in commandInfo.Arguments)
        {
            GetArgumentDeclaration(sb, argument);
        }
        foreach (var subcommand in commandInfo.SubCommands)
        {
            GetSubCommandDeclaration(sb, subcommand);
        }
    }

    internal static void GetArgumentDeclaration(StringBuilder sb, PropInfo propInfo)
    {
        var description = propInfo.Description is null
                              ? "null"
                              : $"\"{propInfo.Description.Replace("\"", "\"\"")}\"";
        sb.Append($$"""
                rootCommand.Add(new Argument<{{propInfo.TypeName}}>("{{propInfo.Name}}")
                {
                    Description = {{description}},
                    Required = {{(propInfo.IsRequiredForCli ? "true" : "false")}}
                });
                """);
    }

    internal static void GetOptionDeclaration(StringBuilder sb, PropInfo propInfo)
    {
        var description = propInfo.Description is null
                              ? "null"
                              : $"\"{propInfo.Description.Replace("\"", "\"\"")}\"";
        sb.Append($$"""
                rootCommand.Add(new Option<{{propInfo.TypeName}}>("--{{propInfo.CliName}}")
                {
                    Description = {{description}},
                    Required = {{(propInfo.IsRequiredForCli ? "true" : "false")}}
                });
                """);
    }



}
