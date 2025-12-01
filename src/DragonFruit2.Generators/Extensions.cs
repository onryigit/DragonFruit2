using Microsoft.CodeAnalysis;
using System.Linq;
using System.Text;

namespace DragonFruit2.GeneratorSupport;

public static class Extensions
{
    extension(ISymbol symbol)
    {
        public T? GetAttributeValue<T>(string namespaceName, string attributeName, string parameterName)
        {
            var attr = symbol.GetAttributes().FirstOrDefault(a =>
                    a.AttributeClass?.Name == attributeName ||
                    a.AttributeClass?.ToDisplayString() == $"{namespaceName}.{parameterName}");

            if (attr == null)
                return default;


            if (attr.ConstructorArguments.Length == 1 && attr.ConstructorArguments[0].Value is T s)
                return s;
            else
            {
                var named = attr.NamedArguments.FirstOrDefault(kv => kv.Key == parameterName);
                if (named.Value.Value is T s2)
                    return s2;
            }
            return default;
        }
    }

    extension(INamedTypeSymbol typeSymbol)
    {
        public string? GetNamespace()
        {
            var ns = typeSymbol.ContainingNamespace;
            if (ns is null || ns.IsGlobalNamespace) return null;
            return ns.ToDisplayString();
        }
    }
}
