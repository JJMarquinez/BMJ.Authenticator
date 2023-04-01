using BMJ.Authenticator.Application.Common.Abstractions;
using BMJ.Authenticator.Application.Common.Instrumentation;
using BMJ.Authenticator.Application.Common.Interfaces;
using BMJ.Authenticator.Domain.Common.Results;
using BMJ.Authenticator.Domain.Entities.Users;
using BMJ.Authenticator.Infrastructure.Common;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;

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
            ApplicationUser user = new ApplicationUser { UserName = userName, Email = email, PhoneNumber = phoneNumber };


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

            ApplicationUser? applicationUser = await GetUserByIdInstrumentedAsync(userId);

            applicationUser.UserName = userName;
            applicationUser.Email = email;
            applicationUser.PhoneNumber = phoneNumber;

            IdentityResult identityResult = await UpdateUserIntrumentedAsync(applicationUser);

            if (identityResult.Succeeded)
                result = Result.Success();
            else
            {
                using Activity? loggingActivity = Telemetry.Source.StartActivity("Logging", System.Diagnostics.ActivityKind.Internal);
                loggingActivity.DisplayName = "Logging errors got from Identity";

                _authLogger.Error<IEnumerable<IdentityError>, ApplicationUser>(
                    "The following errors {@Errors} don't allow delete the user {@applicationUser}",
                    identityResult.Errors,
                    applicationUser);

                loggingActivity.SetTag("Error", identityResult.Errors);
                loggingActivity.Stop();
            }

            return result;
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
            return await DeleteUserAsync(user); 
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
            ApplicationUser? applicationUser = await _userManager.FindByNameAsync(userName);
            if (applicationUser is not null)
            {
                bool isValidPassword = await _userManager.CheckPasswordAsync(applicationUser, password);
                if (isValidPassword)
                {
                    var roles = await _userManager.GetRolesAsync(applicationUser);
                    result = applicationUser.ToUser(roles?.ToArray());
                }
                else
                    _authLogger.Warning<string>("The password ({password}) doesn't match with any user", password);
            }
            else
                _authLogger.Warning<string>("The userName ({userName}) doesn't match with any user", userName);

            return result;
        }

        public bool DoesUserNameNotExist(string userName)
            => _userManager.Users.All(u => u.UserName != userName);

        public bool IsUserIdAssigned(string userId)
            => _userManager.Users.Any(u => u.Id == userId);

        private async ValueTask<ApplicationUser?> GetUserByIdInstrumentedAsync(string UserId)
        {
            using Activity? identityGetUserById = Telemetry.Source.StartActivity("GetUserById", ActivityKind.Internal);
            identityGetUserById.DisplayName = "Identity - GetUserById";

            ApplicationUser? applicationUser = await _userManager.Users.FirstOrDefaultAsync(user => user.Id == UserId);

            identityGetUserById.SetTag("UserId", applicationUser.Id);

            return applicationUser;
        }

        private async ValueTask<IdentityResult> UpdateUserIntrumentedAsync(ApplicationUser? user)
        {
            using Activity? identityUpdateUser = Telemetry.Source.StartActivity("UpdateUser", ActivityKind.Internal);
            identityUpdateUser.DisplayName = "Identity - UpdateUser";

            IdentityResult identityResult = await _userManager.UpdateAsync(user);

            identityUpdateUser.SetTag("Succeeded", identityResult.Succeeded);

            return identityResult;
        }
    }
}
