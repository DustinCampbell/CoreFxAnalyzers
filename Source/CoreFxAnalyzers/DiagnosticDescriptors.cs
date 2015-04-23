using Microsoft.CodeAnalysis;

namespace CoreFxAnalyzers
{
    public static class DiagnosticDescriptors
    {
        public static readonly DiagnosticDescriptor DoNotUseImmutableArrayDefeaultCtor = new DiagnosticDescriptor(
            id: DiagnosticIds.DoNotUseImmutableArrayDefaultCtor,
            title: "Do not use ImmutableArray<T> default contructor",
            messageFormat: "",
            category: DiagnosticCategories.ApiGuidance,
            defaultSeverity: DiagnosticSeverity.Error,
            isEnabledByDefault: true);
    }
}
