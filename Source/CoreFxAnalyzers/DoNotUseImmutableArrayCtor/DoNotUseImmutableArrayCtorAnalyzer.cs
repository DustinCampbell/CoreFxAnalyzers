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
            context.RegisterCompilationStartAction(context1 =>
            {
                var type = context1.Compilation.GetTypeByMetadataName("System.Collections.Immutable.ImmutableArray`1");
                if (type != null)
                {
                    context1.RegisterCodeBlockStartAction<SyntaxKind>(
                        context2 =>
                        {
                            context2.RegisterSyntaxNodeAction(
                                context3 => AnalyzeObjectCreationExpression(context3, type),
                                SyntaxKind.ObjectCreationExpression);
                        });
                }
            });
        }

        private void AnalyzeObjectCreationExpression(SyntaxNodeAnalysisContext context, INamedTypeSymbol type)
        {
            var objectCreationExpression = (ObjectCreationExpressionSyntax)context.Node;

            if (objectCreationExpression.IsType(type, context.SemanticModel))
            {
                context.ReportDiagnostic(
                    Diagnostic.Create(
                        DiagnosticDescriptors.DoNotUseImmutableArrayCtor,
                        objectCreationExpression.Type.GetLocation()));
            }
        }
    }
}
