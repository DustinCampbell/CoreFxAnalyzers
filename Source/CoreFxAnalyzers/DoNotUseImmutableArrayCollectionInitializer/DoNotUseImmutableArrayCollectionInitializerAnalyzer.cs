using System.Collections.Immutable;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Diagnostics;

namespace CoreFxAnalyzers.DoNotUseImmutableArrayCollectionInitializer
{
    [DiagnosticAnalyzer(LanguageNames.CSharp)]
    public class DoNotUseImmutableArrayCollectionInitializerAnalyzer : ImmutableArrayAnalyzer
    {
        public override ImmutableArray<DiagnosticDescriptor> SupportedDiagnostics => ImmutableArray.Create(DiagnosticDescriptors.DoNotUseImmutableArrayCollectionInitializer);

        protected override void RegisterImmutableArrayAction(CompilationStartAnalysisContext context, INamedTypeSymbol immutableArrayType)
        {
            context.RegisterCodeBlockStartAction<SyntaxKind>(
                c => c.RegisterSyntaxNodeAction(
                    c2 => AnalyzeCollectionInitializerExpression(c2, immutableArrayType),
                    SyntaxKind.CollectionInitializerExpression));
        }

        private void AnalyzeCollectionInitializerExpression(SyntaxNodeAnalysisContext context, INamedTypeSymbol type)
        {
            var collectionInitializer = (InitializerExpressionSyntax)context.Node;
            var objectCreationExpression = collectionInitializer.Parent as ObjectCreationExpressionSyntax;

            if (objectCreationExpression?.IsType(type, context.SemanticModel) == true)
            {
                context.ReportDiagnostic(
                    Diagnostic.Create(
                        DiagnosticDescriptors.DoNotUseImmutableArrayCollectionInitializer,
                        collectionInitializer.GetLocation()));
            }
        }
    }
}
