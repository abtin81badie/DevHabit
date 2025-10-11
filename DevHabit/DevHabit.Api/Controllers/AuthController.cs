using DevHabit.Api.Database;
using DevHabit.Api.DTOs.Auth;
using DevHabit.Api.DTOs.Users;
using DevHabit.Api.Entities;
using DevHabit.Api.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;

namespace DevHabit.Api.Controllers;

[ApiController]
[Route("auth")]
[AllowAnonymous]
public sealed class AuthController(
    UserManager<IdentityUser> userManager,
    ApplicationIdentityDbContext identityDbContext,
    ApplicationDbContext applicationDbContext,
    TokenProvider tokenProvider) : ControllerBase
{
    [HttpPost("register")]
    public async Task<IActionResult> Register(RegisterUserDto registerUserDto)
    {
        await using IDbContextTransaction transation = await identityDbContext.Database.BeginTransactionAsync();
        applicationDbContext.Database.SetDbConnection(identityDbContext.Database.GetDbConnection());
        await applicationDbContext.Database.UseTransactionAsync(transation.GetDbTransaction());

        var identityUser = new IdentityUser
        {
            Email = registerUserDto.Email,
            UserName = registerUserDto.Email
        };

        IdentityResult identityResult = await userManager.CreateAsync(identityUser, registerUserDto.Password);

        if (!identityResult.Succeeded)
        {
            Dictionary<string, object?> extensions = new() {
                {
                    "errors", identityResult.Errors.ToDictionary(
                            e => e.Code,
                            e => e.Description)
                }
            };
            return Problem(
                    detail: "Unable to register user, please try again.",
                    statusCode: StatusCodes.Status400BadRequest,
                    extensions: extensions
                );
        }

        User user = registerUserDto.ToEntity(identityUser.Id);

        applicationDbContext.Users.Add(user);

        await applicationDbContext.SaveChangesAsync();

        await transation.CommitAsync();

        TokenRequestDto tokenRequest = new(identityUser.Id, identityUser.Email);
        AccessTokensDto accessTokens = tokenProvider.Create(tokenRequest);

        return Ok(accessTokens);
    }

    [HttpPost("login")]
    public async Task<ActionResult<AccessTokensDto>> Login(LoginUserDto loginUserDto)
    {
        var identityUser = await userManager.FindByEmailAsync(loginUserDto.Email);

        if (identityUser is null || !await userManager.CheckPasswordAsync(identityUser, loginUserDto.Password))
        {
            return Unauthorized();
        }

#pragma warning disable CS8604 // Possible null reference argument.
        var tokenRequest = new TokenRequestDto(identityUser.Id, identityUser.Email);
#pragma warning restore CS8604 // Possible null reference argument.
        AccessTokensDto accessTokens = tokenProvider.Create(tokenRequest);

        return Ok(accessTokens);
    }
}
