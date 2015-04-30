using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CoreFxAnalyzers
{
    internal static class Extensions
    {
        public static bool IsType(this ObjectCreationExpressionSyntax objectCreationExpression, INamedTypeSymbol type, SemanticModel semanticModel) =>
            (semanticModel.GetSymbolInfo(objectCreationExpression.Type).Symbol as INamedTypeSymbol)?.ConstructedFrom == type;
    }
}
