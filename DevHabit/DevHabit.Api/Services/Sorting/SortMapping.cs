namespace DevHabit.Api.Services.Sorting;

// Age DESC -> DateOfBirth ASC
// 30       -> 1995
public sealed record SortMapping (string SortField, string PropertyName, bool Reverse = false);
