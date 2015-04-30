using CoreFxAnalyzers.DoNotUseImmutableArrayCollectionInitializer;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Diagnostics;
using NUnit.Framework;

namespace CoreFxAnalyzers.Tests
{
    [TestFixture]
    public class DoNotUseImmutableArrayCollectionInitializerTests : ImmutableAnalyzerTestFixture
    {
        protected override string LanguageName => LanguageNames.CSharp;
        protected override DiagnosticAnalyzer CreateAnalyzer() => new DoNotUseImmutableArrayCollectionInitializerAnalyzer();

        [Test]
        public void SimpleTest()
        {
            const string markupCode = @"
using System.Collections.Immutable;
class C
{
    void M()
    {
        var a = new ImmutableArray<int> [|{ 1, 2, 3, 4, 5 }|];
    }
}
";

            HasDiagnostic(markupCode, DiagnosticIds.DoNotUseImmutableArrayCollectionInitializer);
        }
    }
}
