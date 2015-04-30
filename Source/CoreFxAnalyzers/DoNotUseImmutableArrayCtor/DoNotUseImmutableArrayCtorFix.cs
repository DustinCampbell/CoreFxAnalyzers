using System.Collections.Immutable;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CodeActions;
using Microsoft.CodeAnalysis.CodeFixes;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CoreFxAnalyzers.DoNotUseImmutableArrayCtor
{
    [ExportCodeFixProvider(LanguageNames.CSharp)]
    public class DoNotUseImmutableArrayCtorFix : CodeFixProvider
    {
        public override ImmutableArray<string> FixableDiagnosticIds => ImmutableArray.Create(DiagnosticIds.DoNotUseImmutableArrayDefaultCtor);

        public override async Task RegisterCodeFixesAsync(CodeFixContext context)
        {
            Debug.Assert(context.Diagnostics.Length == 1);

            var root = await context.Document.GetSyntaxRootAsync(context.CancellationToken);
            var objectCreation = root.FindNode(context.Span) as ObjectCreationExpressionSyntax;

            Debug.Assert(objectCreation != null);
            Debug.Assert(objectCreation.Initializer == null);

            if (objectCreation != null)
            {
                context.RegisterCodeFix(
                    CodeAction.Create("Use ImmutableArray<T>.Empty",
                        c => ChangeToImmutableArrayEmpty(objectCreation, context.Document, c)),
                    context.Diagnostics[0]);
            }
        }

        private static async Task<Document> ChangeToImmutableArrayEmpty(ObjectCreationExpressionSyntax objectCreation, Document document, CancellationToken cancellationToken)
        {
            var newMemberAccess = SyntaxFactory.MemberAccessExpression(
                kind: SyntaxKind.SimpleMemberAccessExpression,
                expression: objectCreation.Type,
                name: SyntaxFactory.IdentifierName("Empty"));

            var oldRoot = await document.GetSyntaxRootAsync(cancellationToken);
            var newRoot = oldRoot.ReplaceNode(objectCreation, newMemberAccess);

            return document.WithSyntaxRoot(newRoot);
        }
    }
}
