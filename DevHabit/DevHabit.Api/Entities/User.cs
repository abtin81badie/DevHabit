namespace DevHabit.Api.Entities;

public sealed class User
{
    public required string Id { get; set; }
    public required string Email { get; set; }
    public required string Name { get; set; }
    public DateTime CreatedAtUtc { get; set; }
    public DateTime? UpdatedAtUtc { get; set; }

    // Azure AD, Cognito, Keycloak, Auth0, etc.
    public required string IdentityId { get; set; }

}
