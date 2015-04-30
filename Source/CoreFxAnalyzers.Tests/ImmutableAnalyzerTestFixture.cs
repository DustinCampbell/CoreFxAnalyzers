using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Text;
using RoslynNUnitLight;

namespace CoreFxAnalyzers.Tests
{
    public abstract class ImmutableAnalyzerTestFixture : AnalyzerTestFixture
    {
        protected new void HasDiagnostic(string markupCode, string diagnosticId)
        {
            Document document;
            TextSpan span;
            TestHelpers.TryGetDocumentAndSpanFromMarkup(markupCode, LanguageName, References.Default, out document, out span);

            HasDiagnostic(document, span, diagnosticId);
        }

        protected new void NoDiagnostic(string code, string diagnosticId)
        {
            var document = TestHelpers.GetDocument(code, LanguageName, References.Default);

            NoDiagnostic(document, diagnosticId);
        }
    }
}
