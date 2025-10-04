using DevHabit.Api.Entities;

namespace DevHabit.Api.DTOs.Habits;

public sealed record CreateHabitDto
{
    public string Name { get; init; } = string.Empty;
    public string? Description { get; init; }

    // Add the 'required' keyword here
    public required HabitType Type { get; init; }

    public required FrequencyDto Frequency { get; init; }
    public required TargetDto Target { get; init; }
    public DateOnly? EndDate { get; init; }
    public MilestoneDto? Milestone { get; init; }
}
