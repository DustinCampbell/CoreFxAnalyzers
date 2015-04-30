using CoreFxAnalyzers.DoNotUseImmutableArrayCtor;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Diagnostics;
using NUnit.Framework;

namespace CoreFxAnalyzers.Tests
{
    [TestFixture]
    public class DoNotUseImmutableArrayDefaultCtorAnalyzerTests : ImmutableAnalyzerTestFixture
    {
        protected override string LanguageName => LanguageNames.CSharp;
        protected override DiagnosticAnalyzer CreateAnalyzer() => new DoNotUseImmutableArrayCtorAnalyzer();

        [Test]
        public void SimpleTest()
        {
            const string markupCode = @"
using System.Collections.Immutable;
class C
{
    void M()
    {
        var a = new [|ImmutableArray<int>|]();
    }
}
";

            HasDiagnostic(markupCode, DiagnosticIds.DoNotUseImmutableArrayDefaultCtor);
        }

        [Test]
        public void NoDiagnosticWhenCollectionInitializerIsPresent()
        {
            const string code = @"
using System.Collections.Immutable;
class C
{
    void M()
    {
        var a = new ImmutableArray<int> { 1, 2, 3, 4, 5 };
    }
}
";

            NoDiagnostic(code, DiagnosticIds.DoNotUseImmutableArrayCollectionInitializer);
        }

    }
}
