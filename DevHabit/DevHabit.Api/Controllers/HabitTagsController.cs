using DevHabit.Api.Database;
using DevHabit.Api.DTOs.HabitTags;
using DevHabit.Api.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DevHabit.Api.Controllers;

[ApiController]
[Route("habits/{habitId}/tags")]
public sealed class HabitTagsController(ApplicationDbContext dbContext): ControllerBase
{
#pragma warning disable CA1307 // Specify StringComparison for clarity
    public static readonly string Name = nameof(HabitsController).Replace("Controller", string.Empty);
#pragma warning restore CA1307 // Specify StringComparison for clarity


    [HttpPut]
    public async Task<ActionResult> UpsertHabitTags(string habitId, UpsertHabitsTagsDto upsertHabitsTagsDto)
    {
        Habit? habit = await dbContext.Habits
            .Include(h => h.HabitTags)
            .FirstOrDefaultAsync(h => h.Id == habitId);
        
        if (habit is null)
            return NotFound();

        var currentTagIds = habit.HabitTags?.Select(ht => ht.TagId).ToHashSet() ?? new HashSet<string>();
        if (currentTagIds.SetEquals(upsertHabitsTagsDto.TagIds))
            return NoContent();

        List<string> existingTagIds = await dbContext
            .Tags
            .Where(t => upsertHabitsTagsDto.TagIds.Contains(t.Id))
            .Select(t => t.Id)
            .ToListAsync();

        if (existingTagIds.Count != upsertHabitsTagsDto.TagIds.Count)
            return BadRequest("One or more tag IDs is Invalid.");

        if (habit.HabitTags != null && upsertHabitsTagsDto.TagIds != null)
            habit.HabitTags.RemoveAll(ht => !upsertHabitsTagsDto.TagIds.Contains(ht.TagId));

#pragma warning disable CS8604 // Possible null reference argument.
        string[] tagIdsToAdd = upsertHabitsTagsDto.TagIds.Except(currentTagIds).ToArray();
#pragma warning restore CS8604 // Possible null reference argument.
#pragma warning disable CS8602 // Dereference of a possibly null reference.
        habit.HabitTags.AddRange(tagIdsToAdd.Select(tagId => new HabitTag
        {
            HabitId = habitId,
            TagId = tagId,
            CreatedAtUtc = DateTime.UtcNow
        }));
#pragma warning restore CS8602 // Dereference of a possibly null reference.

        await dbContext.SaveChangesAsync();

        return Ok();
    }

    [HttpDelete("{tagId}")]
    public async Task<ActionResult> DeleteHabitTag(string habitId, string tagId)
    {
        HabitTag? habitTag = await dbContext.HabitTags
            .SingleOrDefaultAsync(ht => ht.HabitId == habitId && ht.TagId == tagId);

        if (habitTag == null)
            return NotFound();

        dbContext.HabitTags.Remove(habitTag);

        await dbContext.SaveChangesAsync();

        return NoContent();
    }

}
