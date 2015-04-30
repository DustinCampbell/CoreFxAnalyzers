using System;
using System.Collections.Immutable;
using System.Threading.Tasks;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CodeFixes;

namespace CoreFxAnalyzers.DoNotUseImmutableArrayCollectionInitializer
{
    [ExportCodeFixProvider(LanguageNames.CSharp)]
    public class DoNotUseImmutableArrayCollectionInitializerCodeFix : CodeFixProvider
    {
        public override ImmutableArray<string> FixableDiagnosticIds
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public override Task RegisterCodeFixesAsync(CodeFixContext context)
        {
            throw new NotImplementedException();
        }
    }
}
