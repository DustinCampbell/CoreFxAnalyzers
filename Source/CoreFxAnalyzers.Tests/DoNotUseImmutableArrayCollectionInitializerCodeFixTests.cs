using CoreFxAnalyzers.DoNotUseImmutableArrayCollectionInitializer;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CodeFixes;
using NUnit.Framework;

namespace CoreFxAnalyzers.Tests
{
    [TestFixture]
    public class DoNotUseImmutableArrayCollectionInitializerCodeFixTests : ImmutableCodeFixTestFixture
    {
        protected override string LanguageName => LanguageNames.CSharp;
        protected override CodeFixProvider CreateProvider() => new DoNotUseImmutableArrayCollectionInitializerCodeFix();

        [Test]
        public void SimpleTest()
        {
            const string markupCode = @"
using System.Collections.Immutable;
class C
{
    void M()
    {
        var a = new ImmutableArray<int>() [|{ 1, 2, 3, 4, 5 }|];
    }
}
";

            const string expected = @"
using System.Collections.Immutable;
class C
{
    void M()
    {
        var a = ImmutableArray.CreateRange(new int[] { 1, 2, 3, 4, 5 });
    }
}
";

            TestCodeFix(markupCode, expected, DiagnosticDescriptors.DoNotUseImmutableArrayCtor);
        }

    }
}
