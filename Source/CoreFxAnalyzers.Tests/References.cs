using System.Collections.Immutable;
using System.Linq;
using System.Reflection;
using Microsoft.CodeAnalysis;

namespace CoreFxAnalyzers.Tests
{
    internal static class References
    {
        public static ImmutableList<MetadataReference> Default => ImmutableList.Create(
            MetadataReference.CreateFromAssembly(typeof(object).GetTypeInfo().Assembly),
            MetadataReference.CreateFromAssembly(typeof(Enumerable).GetTypeInfo().Assembly),
            MetadataReference.CreateFromAssembly(typeof(ImmutableArray<>).GetTypeInfo().Assembly));
    }
}
