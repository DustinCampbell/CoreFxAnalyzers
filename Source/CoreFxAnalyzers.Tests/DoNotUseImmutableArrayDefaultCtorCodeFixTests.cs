using CoreFxAnalyzers.DoNotUseImmutableArrayCtor;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CodeFixes;
using NUnit.Framework;

namespace CoreFxAnalyzers.Tests
{
    public class DoNotUseImmutableArrayDefaultCtorCodeFixTests : ImmutableCodeFixTestFixture
    {
        protected override string LanguageName => LanguageNames.CSharp;
        protected override CodeFixProvider CreateProvider() => new DoNotUseImmutableArrayCtorFix();

        [Test]
        public void SimpleTest()
        {
            const string markupCode = @"
using System.Collections.Immutable;
class C
{
    void M()
    {
        var a = new [|ImmutableArray<int>()|];
    }
}
";

            const string expected = @"
using System.Collections.Immutable;
class C
{
    void M()
    {
        var a = ImmutableArray<int>.Empty;
    }
}
";

            TestCodeFix(markupCode, expected, DiagnosticDescriptors.DoNotUseImmutableArrayCtor);
        }
    }
}
