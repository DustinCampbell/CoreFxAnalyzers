using System.Collections.Immutable;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CodeActions;
using Microsoft.CodeAnalysis.CodeFixes;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Editing;
using Microsoft.CodeAnalysis.Formatting;

namespace CoreFxAnalyzers.DoNotUseImmutableArrayCollectionInitializer
{
    [ExportCodeFixProvider(LanguageNames.CSharp)]
    public class DoNotUseImmutableArrayCollectionInitializerCodeFix : CodeFixProvider
    {
        public override ImmutableArray<string> FixableDiagnosticIds => ImmutableArray.Create(DiagnosticIds.DoNotUseImmutableArrayCollectionInitializer);

        public override async Task RegisterCodeFixesAsync(CodeFixContext context)
        {
            Debug.Assert(context.Diagnostics.Length == 1);

            var root = await context.Document.GetSyntaxRootAsync(context.CancellationToken);
            var collectionInitializer = root.FindNode(context.Span).FirstAncestorOrSelf<InitializerExpressionSyntax>();

            if (collectionInitializer == null ||
                collectionInitializer.Kind() != SyntaxKind.CollectionInitializerExpression)
            {
                return;
            }

            var objectCreation = collectionInitializer.Parent as ObjectCreationExpressionSyntax;
            if (objectCreation == null)
            {
                return;
            }

            var semanticModel = await context.Document.GetSemanticModelAsync(context.CancellationToken);
            var immutableArrayType = semanticModel.Compilation.GetTypeByMetadataName("System.Collections.Immutable.ImmutableArray");
            if (immutableArrayType == null)
            {
                return;
            }

            var type = semanticModel.GetSymbolInfo(objectCreation.Type, context.CancellationToken).Symbol as INamedTypeSymbol;
            if (type == null ||
                type.Name != "ImmutableArray" ||
                type.TypeArguments.Length != 1)
            {
                return;
            }

            context.RegisterCodeFix(
                CodeAction.Create("Use ImmutableArray.CreateRange()",
                c => ChangeToImmutableArrayCreateRange(objectCreation, collectionInitializer, immutableArrayType, type.TypeArguments[0], context.Document, c)),
                context.Diagnostics[0]);
        }

        private static async Task<Document> ChangeToImmutableArrayCreateRange(
            ObjectCreationExpressionSyntax objectCreation,
            InitializerExpressionSyntax initializer,
            INamedTypeSymbol immutableArrayType,
            ITypeSymbol elementType,
            Document document,
            CancellationToken cancellationToken)
        {
            var generator = SyntaxGenerator.GetGenerator(document);

            var arrayElementType = (TypeSyntax)generator.TypeExpression(elementType);
            var arrayType = SyntaxFactory.ArrayType(arrayElementType, 
                SyntaxFactory.SingletonList(
                    SyntaxFactory.ArrayRankSpecifier(
                        SyntaxFactory.SingletonSeparatedList(
                            (ExpressionSyntax)SyntaxFactory.OmittedArraySizeExpression()))));

            var arrayCreationExpression = SyntaxFactory.ArrayCreationExpression(
                type: arrayType,
                initializer: SyntaxFactory.InitializerExpression(
                    kind: SyntaxKind.ArrayInitializerExpression,
                    expressions: initializer.Expressions))
                .WithAdditionalAnnotations(Formatter.Annotation);
            
            var type = generator.TypeExpression(immutableArrayType);
            var memberAccess = generator.MemberAccessExpression(type, "CreateRange");
            var invocation = generator.InvocationExpression(memberAccess, arrayCreationExpression);

            var oldRoot = await document.GetSyntaxRootAsync(cancellationToken);
            var newRoot = oldRoot.ReplaceNode(objectCreation, invocation);

            return document.WithSyntaxRoot(newRoot);
        }
    }
}