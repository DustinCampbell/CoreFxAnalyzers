using System.Collections.Immutable;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Diagnostics;

namespace CoreFxAnalyzers.DoNotUseImmutableArrayCtor
{
    [DiagnosticAnalyzer(LanguageNames.CSharp)]
    public class DoNotUseImmutableArrayCtorAnalyzer : DiagnosticAnalyzer
    {
        public override ImmutableArray<DiagnosticDescriptor> SupportedDiagnostics =>
            ImmutableArray.Create(DiagnosticDescriptors.DoNotUseImmutableArrayCtor);

        public override void Initialize(AnalysisContext context)
        {
            context.RegisterCompilationStartAction(outerContext =>
            {
                var type = outerContext.Compilation.GetTypeByMetadataName("System.Collections.Immutable.ImmutableArray`1");
                if (type != null)
                {
                    context.RegisterSyntaxNodeAction(
                        innerContext => AnalyzeObjectCreationExpression(innerContext, type),
                        SyntaxKind.ObjectCreationExpression);
                }
            });
        }

        private void AnalyzeObjectCreationExpression(SyntaxNodeAnalysisContext context, INamedTypeSymbol type)
        {
            var objectCreationExpression = (ObjectCreationExpressionSyntax)context.Node;

            var objectCreationType = context.SemanticModel.GetSymbolInfo(objectCreationExpression.Type).Symbol as INamedTypeSymbol;
            if (objectCreationType?.ConstructedFrom != type)
            {
                return;
            }

            context.ReportDiagnostic(
                Diagnostic.Create(
                    DiagnosticDescriptors.DoNotUseImmutableArrayCtor,
                    objectCreationExpression.Type.GetLocation()));
        }
    }
}
