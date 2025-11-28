using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System.Collections.Generic;
using System.Linq;
using DragonFruit2.GeneratorSupport;
using System.Text;

namespace DragonFruit2.Generators
{
    [Generator]
    public sealed partial class DragonFruit2Generator : IIncrementalGenerator
    {
        private static DragonFruit2Builder builder = new DragonFruit2Builder();

        private static int indentSize = 4;

        public void Initialize(IncrementalGeneratorInitializationContext context)
        {
            var parseArgsInvocations = context.SyntaxProvider
                .CreateSyntaxProvider(
                    predicate: static (node, _) => builder.InitialFilter (node),
                    transform: static (ctx, _) => builder.Transform(ctx))
                .WithTrackingName("ParseArgsInvocations")
                .Where(static s => s is not null)
                .Select(static (s, _) => s!) // Quiet nullability warning
                .Collect();

            context.RegisterSourceOutput(parseArgsInvocations, static (spc, collected) =>
            {
                builder.OutputSource(spc, collected);
            });
        }

    }

}
