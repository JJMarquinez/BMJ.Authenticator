using BMJ.Authenticator.Adapter.Common.Abstractions;
using BMJ.Authenticator.Application.Common.Interfaces;
using BMJ.Authenticator.Domain.Common.Results;
using BMJ.Authenticator.Domain.Entities.Users;
using BMJ.Authenticator.Adapter.Common;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace BMJ.Authenticator.Infrastructure.Identity
{
    public class IdentityService : IIdentityService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IAuthLogger _authLogger;

        public IdentityService(
            UserManager<ApplicationUser> userManager,
            IAuthLogger authLogger)
        {
            _userManager = userManager;
            _authLogger = authLogger;
        }

        public async Task<Result<List<User>?>> GetAllUserAsync()
        {
            IEnumerable<ApplicationUser> applicationUsers = _userManager.Users.AsEnumerable();
            List<User> userList= new List<User>();
            foreach (ApplicationUser applicationUser in applicationUsers)
            {
                var roles = await _userManager.GetRolesAsync(applicationUser);
                userList.Add(applicationUser.ToUser(roles?.ToArray()));
            }

            if (userList.Count() == 0)
                _authLogger.Warning("It doesn't exist any user.");

            return userList.Count() > 0
                ? userList
                : Result.Failure<List<User>?>(InfrastructureError.Identity.ItDoesNotExistAnyUser);
        }

        public async Task<Result<User?>> GetUserByIdAsync(string userId)
        {
            ApplicationUser? applicationUser = await _userManager.Users.FirstOrDefaultAsync(user => user.Id == userId);
            var roles = await _userManager.GetRolesAsync(applicationUser);
            User user = applicationUser.ToUser(roles?.ToArray());

            return user;
        }

        public async Task<Result<string?>> CreateUserAsync(string userName, string password, string email, string? phoneNumber)
        {
            Result<string?> result = Result.Failure<string?>(InfrastructureError.Identity.UserWasNotCreated); 
            ApplicationUser user = ApplicationUser.Builder()
                .WithUserName(userName)
                .WithEmail(email)
                .WithPhoneNumber(phoneNumber)
                .Build();

            IdentityResult identityResult = await _userManager.CreateAsync(user, password);
            if (identityResult.Succeeded)
                result = Result.Success<string?>(user.Id);
            else
                _authLogger.Error<IEnumerable<IdentityError>, ApplicationUser>(
                    "The following errors {@Errors} don't allow delete the user {@user}",
                    identityResult.Errors,
                    user);
            
            return result;
        }

        public async Task<Result> UpdateUserAsync(string userId, string userName, string email, string? phoneNumber)
        {
            Result result = Result.Failure(InfrastructureError.Identity.UserWasNotUpdated);

            ApplicationUser? applicationUser = await _userManager.Users.FirstOrDefaultAsync(user => user.Id == userId);

            applicationUser.UserName = userName;
            applicationUser.Email = email;
            applicationUser.PhoneNumber = phoneNumber;

            IdentityResult identityResult = await _userManager.UpdateAsync(applicationUser);

            if (identityResult.Succeeded)
                result = Result.Success();
            else
            {
                _authLogger.Error<IEnumerable<IdentityError>, ApplicationUser>(
                    "The following errors {@Errors} don't allow delete the user {@applicationUser}",
                    identityResult.Errors,
                    applicationUser);
            }

            return result;
        }

        public async Task<Result> DeleteUserAsync(string userId)
        {
            ApplicationUser? user = await _userManager.Users.SingleOrDefaultAsync(u => u.Id == userId);
            IdentityResult identityResult = await _userManager.DeleteAsync(user);

            if (!identityResult.Succeeded)
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
            ApplicationUser? applicationUser = await _userManager.FindByNameAsync(userName);

            if (applicationUser == default || !await _userManager.CheckPasswordAsync(applicationUser, password))
                _authLogger.Warning("Invalid username or password!");
            else
            {
                var roles = await _userManager.GetRolesAsync(applicationUser);
                result = applicationUser.ToUser(roles?.ToArray());
            }

            return result;
        }

        public bool DoesUserNameNotExist(string userName)
            => _userManager.Users.All(u => u.UserName != userName);

        public bool IsUserIdAssigned(string userId)
            => _userManager.Users.Any(u => u.Id == userId);
    }
}
