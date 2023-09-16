using BMJ.Authenticator.Infrastructure.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace BMJ.Authenticator.ToolKit.Identity.UserOperators;

public class UserOperator : IUserOperator
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly RoleManager<IdentityRole> _roleManager;

    public UserOperator(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager)
    {
        _userManager = userManager;
        _roleManager = roleManager;
    }

    public async ValueTask<string?> AddAsync(ApplicationUser applicationUser, string password, string[] roles)
    {
        string? userId = null!;
        ApplicationUser? user = null!;
        var userResult = await _userManager.CreateAsync(applicationUser, password).ConfigureAwait(false);

        if (userResult.Succeeded)
        {
            user = await _userManager.Users.FirstOrDefaultAsync(user => user.UserName == applicationUser.UserName).ConfigureAwait(false);
            userId = user?.Id;

            if (roles.Any())
            {
                foreach (var role in roles)
                {
                    var roleResult = await _roleManager.CreateAsync(new IdentityRole(role)).ConfigureAwait(false);
                    if (roleResult.Succeeded)
                        await _userManager.AddToRolesAsync(user!, new[] { role }).ConfigureAwait(false);
                }
            }
        }

        return userId;
    }

    public void Dispose()
    {
        _userManager.Dispose();
        _roleManager.Dispose();
    }
}