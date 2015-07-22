using System.Collections.Immutable;
using System.Linq;
using System.Reflection;
using CoreFxAnalyzers.Tests.Reflection;
using Microsoft.CodeAnalysis;

namespace CoreFxAnalyzers.Tests
{
    internal static class References
    {
        public static ImmutableList<MetadataReference> Default => ImmutableList.Create<MetadataReference>(
            MetadataReference.CreateFromFile(typeof(object).GetTypeInfo().Assembly.GetLocation()),
            MetadataReference.CreateFromFile(typeof(Enumerable).GetTypeInfo().Assembly.GetLocation()),
            MetadataReference.CreateFromFile(typeof(ImmutableArray<>).GetTypeInfo().Assembly.GetLocation()));
    }
}
