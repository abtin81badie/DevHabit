namespace DevHabit.Api.Services.Sorting;

public sealed class SortMappingDefinition<TSource, TDestination> : ISortMappingDefinition
{
    public required IReadOnlyList<SortMapping> Mappings { get; init; }
}