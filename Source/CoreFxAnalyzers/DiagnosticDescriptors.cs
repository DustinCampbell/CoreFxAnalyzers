using Microsoft.CodeAnalysis;

namespace CoreFxAnalyzers
{
    public static class DiagnosticDescriptors
    {
        public static readonly DiagnosticDescriptor DoNotUseImmutableArrayCtor = new DiagnosticDescriptor(
            id: DiagnosticIds.DoNotUseImmutableArrayDefaultCtor,
            title: "Do not use ImmutableArray<T> contructor",
            messageFormat: "Do not use ImmutableArray<T> contructor",
            category: DiagnosticCategories.ApiGuidance,
            defaultSeverity: DiagnosticSeverity.Error,
            isEnabledByDefault: true);

        public static readonly DiagnosticDescriptor DoNotUseImmutableArrayCollectionInitializer = new DiagnosticDescriptor(
            id: DiagnosticIds.DoNotUseImmutableArrayCollectionInitializer,
            title: "Do not use collection initializer with ImmutableArray<T>",
            messageFormat: "Do not use collection initializer with ImmutableArray<T>",
            category: DiagnosticCategories.ApiGuidance,
            defaultSeverity: DiagnosticSeverity.Error,
            isEnabledByDefault: true);
    }
}
