using CoreFxAnalyzers.DoNotUseImmutableArrayDefaultCtor;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Diagnostics;
using NUnit.Framework;
using RoslynNUnitLight;

namespace CoreFxAnalyzers.Tests
{
    [TestFixture]
    public class DoNotUseImmutableArrayDefaultCtorTests : AnalyzerTestFixture
    {
        protected override string LanguageName => LanguageNames.CSharp;
        protected override DiagnosticAnalyzer CreateAnalyzer() => new DoNotUseImmutableArrayDefaultCtorAnalyzer();

        [Test]
        public void SimpleTest()
        {
            const string code = @"
using System.Collections.Immutable;
class C
{
    void M()
    {
        var a = [|new ImmutableArray<int>()|];
    }
}
";

            HasDiagnostic(code, DiagnosticIds.DoNotUseImmutableArrayDefaultCtor);
        }
    }
}
