using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Diagnostics;

namespace CoreFxAnalyzers
{
    public abstract class ImmutableArrayAnalyzer : DiagnosticAnalyzer
    {
        public override void Initialize(AnalysisContext context)
        {
            context.RegisterCompilationStartAction(c =>
            {
                var immutableArrayOfTType = c.Compilation.GetTypeByMetadataName("System.Collections.Immutable.ImmutableArray`1");
                if (immutableArrayOfTType != null)
                {
                    RegisterImmutableArrayAction(c, immutableArrayOfTType);
                }
            });

        }

        protected abstract void RegisterImmutableArrayAction(CompilationStartAnalysisContext context, INamedTypeSymbol immutableArrayType);
    }
}
