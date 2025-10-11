using DevHabit.Api.DTOs.Auth;
using DevHabit.Api.Entities;

namespace DevHabit.Api.DTOs.Users;

public static class UserMappings
{
    public static User ToEntity(this RegisterUserDto registerUserDto, string identityId)
    {
        return new User
        {
            Id = $"u_{Guid.CreateVersion7()}",
            Name = registerUserDto.Name,
            Email = registerUserDto.Email,
            CreatedAtUtc = DateTime.UtcNow,
            IdentityId = identityId
        };
    }
}