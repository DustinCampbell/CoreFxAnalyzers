using System.Collections.Immutable;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Diagnostics;

namespace CoreFxAnalyzers.DoNotUseImmutableArrayCtor
{
    [DiagnosticAnalyzer(LanguageNames.CSharp)]
    public class DoNotUseImmutableArrayCtorAnalyzer : ImmutableArrayAnalyzer
    {
        public override ImmutableArray<DiagnosticDescriptor> SupportedDiagnostics =>
            ImmutableArray.Create(DiagnosticDescriptors.DoNotUseImmutableArrayCtor);

        protected override void RegisterImmutableArrayAction(CompilationStartAnalysisContext context, INamedTypeSymbol immutableArrayType)
        {
            context.RegisterCodeBlockStartAction<SyntaxKind>(
                c => c.RegisterSyntaxNodeAction(
                    c2 => AnalyzeObjectCreationExpression(c2, immutableArrayType),
                    SyntaxKind.ObjectCreationExpression));
        }

        private void AnalyzeObjectCreationExpression(SyntaxNodeAnalysisContext context, INamedTypeSymbol type)
        {
            var objectCreationExpression = (ObjectCreationExpressionSyntax)context.Node;

            if (objectCreationExpression.Initializer?.Kind() != SyntaxKind.CollectionInitializerExpression &&
                objectCreationExpression.IsType(type, context.SemanticModel))
            {
                context.ReportDiagnostic(
                    Diagnostic.Create(
                        DiagnosticDescriptors.DoNotUseImmutableArrayCtor,
                        objectCreationExpression.Type.GetLocation()));
            }
        }
    }
}
