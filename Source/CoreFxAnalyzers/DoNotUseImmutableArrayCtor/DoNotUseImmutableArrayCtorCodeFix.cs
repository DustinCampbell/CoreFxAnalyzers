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

namespace CoreFxAnalyzers.DoNotUseImmutableArrayCtor
{
    [ExportCodeFixProvider(LanguageNames.CSharp)]
    public class DoNotUseImmutableArrayCtorCodeFix : CodeFixProvider
    {
        public override ImmutableArray<string> FixableDiagnosticIds => ImmutableArray.Create(DiagnosticIds.DoNotUseImmutableArrayDefaultCtor);

        public override async Task RegisterCodeFixesAsync(CodeFixContext context)
        {
            Debug.Assert(context.Diagnostics.Length == 1);

            var root = await context.Document.GetSyntaxRootAsync(context.CancellationToken);
            var objectCreationExpression = root.FindNode(context.Span).FirstAncestorOrSelf<ObjectCreationExpressionSyntax>();

            Debug.Assert(objectCreationExpression != null);
            Debug.Assert(objectCreationExpression.Initializer == null);

            if (objectCreationExpression != null)
            {
                context.RegisterCodeFix(
                    CodeAction.Create("Use ImmutableArray<T>.Empty",
                        c => ChangeToImmutableArrayEmpty(objectCreationExpression, context.Document, c)),
                    context.Diagnostics[0]);
            }
        }

        private static async Task<Document> ChangeToImmutableArrayEmpty(ObjectCreationExpressionSyntax objectCreation, Document document, CancellationToken cancellationToken)
        {
            var generator = SyntaxGenerator.GetGenerator(document);
            var memberAccess = generator.MemberAccessExpression(objectCreation.Type, "Empty");

            var oldRoot = await document.GetSyntaxRootAsync(cancellationToken);
            var newRoot = oldRoot.ReplaceNode(objectCreation, memberAccess);

            return document.WithSyntaxRoot(newRoot);
        }
    }
}
