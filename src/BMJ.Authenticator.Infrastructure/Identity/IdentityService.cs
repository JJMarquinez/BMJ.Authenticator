using BMJ.Authenticator.Adapter.Common.Abstractions;
using BMJ.Authenticator.Adapter.Common;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;
using BMJ.Authenticator.Application.Common.Models.Results;

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

        public async Task<ResultDto<string?>> GetAllUserAsync()
        {
            IEnumerable<ApplicationUser> applicationUsers = _userManager.Users.ToList();
            List<UserIdentification> userList = new List<UserIdentification>();
            foreach (ApplicationUser applicationUser in applicationUsers)
            {
                var roles = await _userManager.GetRolesAsync(applicationUser);
                userList.Add(applicationUser.ToUserIdentification(roles?.ToArray()));
            }

            if (userList.Count() == 0)
                _authLogger.Warning("It doesn't exist any user.");

            return userList.Count() > 0
                ? ResultDto<string?>.NewSuccess<string?>(JsonSerializer.Serialize(userList))
                : ResultDto<string?>.NewFailure<string?>(InfrastructureError.Identity.ItDoesNotExistAnyUser);
        }

        public async Task<ResultDto<string?>> GetUserByIdAsync(string userId)
        {
            ApplicationUser? applicationUser = await _userManager.Users.FirstOrDefaultAsync(user => user.Id == userId);
            var roles = await _userManager.GetRolesAsync(applicationUser!);
            UserIdentification user = applicationUser!.ToUserIdentification(roles?.ToArray());

            return ResultDto<string?>.NewSuccess<string?>(JsonSerializer.Serialize(user));
        }

        public async Task<ResultDto<string?>> CreateUserAsync(string userName, string password, string email, string? phoneNumber)
        {
            ResultDto<string?> result = ResultDto<string?>.NewFailure<string?>(InfrastructureError.Identity.UserWasNotCreated); 
            ApplicationUser user = ApplicationUser.Builder()
                .WithUserName(userName)
                .WithEmail(email)
                .WithPhoneNumber(phoneNumber)
                .Build();

            IdentityResult identityResult = await _userManager.CreateAsync(user, password);
            if (identityResult.Succeeded)
                result = ResultDto<string?>.NewSuccess<string?>(user.Id);
            else
                _authLogger.Error("The following errors {@Errors} don't allow delete the user {@user}",
                    identityResult.Errors,
                    user);
            
            return result;
        }

        public async Task<ResultDto> UpdateUserAsync(string userId, string userName, string email, string? phoneNumber)
        {
            ResultDto result = ResultDto.NewFailure(InfrastructureError.Identity.UserWasNotUpdated);

            ApplicationUser? applicationUser = await _userManager.Users.FirstOrDefaultAsync(user => user.Id == userId);

            applicationUser!.UserName = userName;
            applicationUser.Email = email;
            applicationUser.PhoneNumber = phoneNumber;

            IdentityResult identityResult = await _userManager.UpdateAsync(applicationUser);

            if (identityResult.Succeeded)
                result = ResultDto.NewSuccess();
            else
            {
                _authLogger.Error("The following errors {@Errors} don't allow delete the user {@applicationUser}",
                    identityResult.Errors,
                    applicationUser);
            }

            return result;
        }

        public async Task<ResultDto> DeleteUserAsync(string userId)
        {
            ApplicationUser? user = await _userManager.Users.SingleOrDefaultAsync(u => u.Id == userId);
            IdentityResult identityResult = await _userManager.DeleteAsync(user!);

            if (!identityResult.Succeeded)
                _authLogger.Error("The following errors {@Errors} don't allow delete the user {@user}",
                    identityResult.Errors,
                    user!);

            return identityResult.Succeeded
                ? ResultDto.NewSuccess()
                : ResultDto.NewFailure(InfrastructureError.Identity.UserWasNotDeleted);
        }

        public async Task<ResultDto<string?>> AuthenticateMemberAsync(string userName, string password)
        {
            ResultDto<string?> result = ResultDto<string?>.NewFailure<string?>(InfrastructureError.Identity.UserNameOrPasswordNotValid);
            ApplicationUser? applicationUser = await _userManager.FindByNameAsync(userName);

            if (applicationUser == default || !await _userManager.CheckPasswordAsync(applicationUser, password))
                _authLogger.Warning("Invalid username or password!");
            else
            {
                var roles = await _userManager.GetRolesAsync(applicationUser);
                result = ResultDto<string?>.NewSuccess<string?>(JsonSerializer.Serialize(applicationUser.ToUserIdentification(roles?.ToArray())));
            }

            return result;
        }

        public bool DoesUserNameNotExist(string userName)
            => _userManager.Users.All(u => u.UserName != userName);

        public bool IsUserIdAssigned(string userId)
            => _userManager.Users.Any(u => u.Id == userId);
    }
}
