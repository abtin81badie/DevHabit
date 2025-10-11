using Newtonsoft.Json;

namespace DevHabit.Api.DTOs.Habits;

public sealed record HabitWithTagsDto : HabitDto
{
    [JsonProperty(Order = int.MaxValue)]
    public required IReadOnlyList<string> Tags { get; init; }
}
