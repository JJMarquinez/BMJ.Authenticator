using BMJ.Authenticator.Application.Common.Abstractions;
using BMJ.Authenticator.Application.Common.Interfaces;
using BMJ.Authenticator.Application.Common.Models;
using BMJ.Authenticator.Domain.Common.Results;
using BMJ.Authenticator.Domain.Entities.Users;
using BMJ.Authenticator.Infrastructure.Common;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace BMJ.Authenticator.Infrastructure.Identity
{
    public class IdentityService : IIdentityService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IAuthLogger _authLogger;

        public IdentityService(
            UserManager<ApplicationUser> userManager,
            IUserClaimsPrincipalFactory<ApplicationUser> userClaimsPrincipalFactory,
            IAuthorizationService authorizationService,
            IAuthLogger authLogger)
        {
            _userManager = userManager;
            _authLogger = authLogger;
        }

        public async Task<Result<List<User>?>> GetAllUserAsync()
        {
            IEnumerable<ApplicationUser> users = _userManager.Users.AsEnumerable();
            List<User> userList= new List<User>();
            foreach (ApplicationUser user in users)
            {
                var roles = await _userManager.GetRolesAsync(user);
                userList.Add(user.ToApplicationUser(roles?.ToArray()));
            }

            if (userList.Count() == 0)
                _authLogger.Warning("It doesn't exist any user");

            return userList.Count() > 0
                ? userList
                : Result.Failure<List<User>?>(InfrastructureError.Identity.ItDoesNotExistAnyUser);
        }

        public async Task<Result<User?>> GetUserByNameAsync(string userName)
        {
            ApplicationUser? applicationUser = await _userManager.Users.FirstOrDefaultAsync(user => user.UserName == userName);
            User? user = null;

            if (applicationUser is null)
                _authLogger.Warning<string>("The user with name {userName} wasn't found, it is not possible to get the user name", userName);
            else
            {
                var roles = await _userManager.GetRolesAsync(applicationUser);
                user = applicationUser.ToApplicationUser(roles?.ToArray());
            }

            return user is null
                ? Result.Failure<User?>(InfrastructureError.Identity.UserWasNotFound)
                : user;
        }

        public async Task<Result<string?>> GetUserNameAsync(string userId)
        {
            ApplicationUser? user = await _userManager.Users.SingleOrDefaultAsync(u => u.Id == userId);

            if (user is null)
                _authLogger.Warning<string>("The user with id {userId} wasn't found, it is not possible to get the user name", userId);

            return user is null
                ? Result.Failure<string>(InfrastructureError.Identity.UserWasNotFound)
                : user.UserName;
        }

        public async Task<Result> CreateUserAsync(string userName, string password)
        {
            ApplicationUser user = new ApplicationUser { UserName = userName };

            IdentityResult identityResult = await _userManager.CreateAsync(user, password);

            if (!identityResult.Succeeded)
                _authLogger.Error<IEnumerable<IdentityError>, ApplicationUser>(
                    "The following errors {@Errors} don't allow delete the user {@user}",
                    identityResult.Errors,
                    user);

            return identityResult.Succeeded
                ? Result.Success()
                : Result.Failure(InfrastructureError.Identity.UserWasNotDeleted);
        }

        public async Task<Result<bool>> IsInRoleAsync(string userId, string role)
        {
            ApplicationUser? user = await _userManager.Users.SingleOrDefaultAsync(u => u.Id == userId);

            if (user is null)
                _authLogger.Warning<string>("The user with id {userId} wasn't found, it is not possible to check what roles he has", userId);

            return user is null
                ? Result.Failure<bool>(InfrastructureError.Identity.UserWasNotFound)
                : await _userManager.IsInRoleAsync(user, role);
        }

        public async Task<Result> DeleteUserAsync(string userId)
        {
            ApplicationUser? user = await _userManager.Users.SingleOrDefaultAsync(u => u.Id == userId);

            if(user is null)
                _authLogger.Warning<string>("The user with id {userId} wasn't delete cause he wasn't found", userId);

            return user != null 
                ? await DeleteUserAsync(user) 
                : Result.Failure(InfrastructureError.Identity.UserWasNotFound);
        }

        private async Task<Result> DeleteUserAsync(ApplicationUser user)
        {
            IdentityResult identityResult = await _userManager.DeleteAsync(user);

            if(!identityResult.Succeeded) 
                _authLogger.Error<IEnumerable<IdentityError>, ApplicationUser>(
                    "The following errors {@Errors} don't allow delete the user {@user}",
                    identityResult.Errors, 
                    user);
                
            return identityResult.Succeeded
                ? Result.Success()
                : Result.Failure(InfrastructureError.Identity.UserWasNotDeleted);
        }

        public async Task<Result<User?>> AuthenticateMemberAsync(string userName, string password)
        {
            Result<User?> result = Result.Failure<User?>(InfrastructureError.Identity.UserNameOrPasswordNotValid);
            ApplicationUser? user = await _userManager.FindByNameAsync(userName);
            if (user is not null)
            {
                bool isValidPassword = await _userManager.CheckPasswordAsync(user, password);
                if (isValidPassword)
                {
                    var roles = await _userManager.GetRolesAsync(user);
                    result = user.ToApplicationUser(roles?.ToArray());
                }
                else
                    _authLogger.Warning<string>("The password ({password}) doesn't match with any user", password);
            }
            else
                _authLogger.Warning<string>("The userName ({userName}) doesn't match with any user", userName);

            return result;
        }
    }
}
