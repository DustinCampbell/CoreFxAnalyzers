using System;
using System.Collections.Immutable;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Diagnostics;

namespace CoreFxAnalyzers.DoNotUseImmutableArrayCollectionInitializer
{
    [DiagnosticAnalyzer(LanguageNames.CSharp)]
    public class DoNotUseImmutableArrayCollectionInitializerAnalyzer : DiagnosticAnalyzer
    {
        public override ImmutableArray<DiagnosticDescriptor> SupportedDiagnostics => ImmutableArray.Create(DiagnosticDescriptors.DoNotUseImmutableArrayCollectionInitializer);

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
                                context3 => AnalyzeCollectionInitializerExpression(context3, type),
                                SyntaxKind.CollectionInitializerExpression);
                        });
                }
            });
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
