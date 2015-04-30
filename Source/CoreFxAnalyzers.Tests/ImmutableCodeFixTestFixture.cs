using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Text;
using RoslynNUnitLight;

namespace CoreFxAnalyzers.Tests
{
    public abstract class ImmutableCodeFixTestFixture : CodeFixTestFixture
    {
        protected new void TestCodeFix(string markupCode, string expected, DiagnosticDescriptor descriptor)
        {
            Document document;
            TextSpan span;
            TestHelpers.TryGetDocumentAndSpanFromMarkup(markupCode, LanguageName, References.Default, out document, out span);

            TestCodeFix(document, span, expected, descriptor);
        }
    }
}
