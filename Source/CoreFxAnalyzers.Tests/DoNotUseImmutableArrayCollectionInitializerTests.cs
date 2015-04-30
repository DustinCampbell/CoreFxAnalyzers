using System.Collections.Immutable;
using System.Linq;
using System.Reflection;
using CoreFxAnalyzers.DoNotUseImmutableArrayCollectionInitializer;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Diagnostics;
using Microsoft.CodeAnalysis.Text;
using NUnit.Framework;
using RoslynNUnitLight;

namespace CoreFxAnalyzers.Tests
{
    [TestFixture]
    public class DoNotUseImmutableArrayCollectionInitializerTests : AnalyzerTestFixture
    {
        protected override string LanguageName => LanguageNames.CSharp;
        protected override DiagnosticAnalyzer CreateAnalyzer() => new DoNotUseImmutableArrayCollectionInitializerAnalyzer();

        [Test]
        public void SimpleTest()
        {
            const string code = @"
using System.Collections.Immutable;
class C
{
    void M()
    {
        var a = new ImmutableArray<int> [|{ 1, 2, 3, 4, 5 }|];
    }
}
";

            var references = ImmutableList.Create(
                MetadataReference.CreateFromAssembly(typeof(object).GetTypeInfo().Assembly),
                MetadataReference.CreateFromAssembly(typeof(Enumerable).GetTypeInfo().Assembly),
                MetadataReference.CreateFromAssembly(typeof(ImmutableArray<>).GetTypeInfo().Assembly));

            Document document;
            TextSpan span;
            TestHelpers.TryGetDocumentAndSpanFromMarkup(code, LanguageName, references, out document, out span);

            HasDiagnostic(document, span, DiagnosticIds.DoNotUseImmutableArrayCollectionInitializer);
        }
    }
}
