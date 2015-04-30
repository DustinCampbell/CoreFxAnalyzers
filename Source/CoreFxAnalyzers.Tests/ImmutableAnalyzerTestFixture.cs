using System.Collections.Immutable;
using System.Linq;
using System.Reflection;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Text;
using RoslynNUnitLight;

namespace CoreFxAnalyzers.Tests
{
    public abstract class ImmutableAnalyzerTestFixture : AnalyzerTestFixture
    {
        protected new void HasDiagnostic(string markupCode, string diagnosticId)
        {
            var references = ImmutableList.Create(
                MetadataReference.CreateFromAssembly(typeof(object).GetTypeInfo().Assembly),
                MetadataReference.CreateFromAssembly(typeof(Enumerable).GetTypeInfo().Assembly),
                MetadataReference.CreateFromAssembly(typeof(ImmutableArray<>).GetTypeInfo().Assembly));

            Document document;
            TextSpan span;
            TestHelpers.TryGetDocumentAndSpanFromMarkup(markupCode, LanguageName, references, out document, out span);

            HasDiagnostic(document, span, diagnosticId);
        }

        protected new void NoDiagnostic(string code, string diagnosticId)
        {
            var references = ImmutableList.Create(
                MetadataReference.CreateFromAssembly(typeof(object).GetTypeInfo().Assembly),
                MetadataReference.CreateFromAssembly(typeof(Enumerable).GetTypeInfo().Assembly),
                MetadataReference.CreateFromAssembly(typeof(ImmutableArray<>).GetTypeInfo().Assembly));

            var document = TestHelpers.GetDocument(code, LanguageName, references);

            NoDiagnostic(document, diagnosticId);
        }
    }
}
