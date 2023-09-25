﻿using BMJ.Authenticator.Adapter.Common.Abstractions;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;
using BMJ.Authenticator.Application.Common.Models.Results;
using BMJ.Authenticator.Application.Common.Models.Errors.Builders;
using BMJ.Authenticator.Infrastructure.Properties;
using BMJ.Authenticator.Infrastructure.Identity.Builders;
using BMJ.Authenticator.Application.Common.Models.Results.Builders;

namespace BMJ.Authenticator.Infrastructure.Identity
{
    public class IdentityService : IIdentityService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IAuthLogger _authLogger;
        private readonly IResultDtoGenericBuilder _resultDtoGenericBuilder;
        private readonly IResultDtoBuilder _resultDtoBuilder;
        private readonly IErrorDtoBuilder _errorDtoBuilder;
        private readonly IApplicationUserBuilder _applicationUserBuilder;

        public IdentityService(
            UserManager<ApplicationUser> userManager,
            IAuthLogger authLogger,
            IErrorDtoBuilder errorDtoBuilder,
            IApplicationUserBuilder applicationUserBuilder,
            IResultDtoGenericBuilder resultDtoGenericBuilder,
            IResultDtoBuilder resultDtoBuilder)
        {
            _userManager = userManager;
            _authLogger = authLogger;
            _errorDtoBuilder = errorDtoBuilder;
            _applicationUserBuilder = applicationUserBuilder;
            _resultDtoGenericBuilder = resultDtoGenericBuilder;
            _resultDtoBuilder = resultDtoBuilder;
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

            var error = _errorDtoBuilder
                .WithCode(string.Format("{0}{1}", InfrastructureString.ErrorCodeInvalidOperationPrefix, InfrastructureString.ErrorGetAllUserCode))
                .WithTitle(InfrastructureString.ErrorGetAllUserTitle)
                .WithDetail(InfrastructureString.ErrorGetAllUserDetail)
                .WithHttpStatusCode(int.Parse(InfrastructureString.ErrorGetAllUserHttpStatusCode))
                .Build();

            return userList.Count() > 0
                ? _resultDtoGenericBuilder.BuildSuccess<string?>(JsonSerializer.Serialize(userList))
                : _resultDtoGenericBuilder.BuildFailure<string?>(error);
        }

        public async Task<ResultDto<string?>> GetUserByIdAsync(string userId)
        {
            ApplicationUser? applicationUser = await _userManager.Users.FirstOrDefaultAsync(user => user.Id == userId);
            var roles = await _userManager.GetRolesAsync(applicationUser!);
            UserIdentification user = applicationUser!.ToUserIdentification(roles?.ToArray());

            return _resultDtoGenericBuilder.BuildSuccess<string?>(JsonSerializer.Serialize(user));
        }

        public async Task<ResultDto> CreateUserAsync(string userName, string password, string email, string? phoneNumber)
        {
            var error = _errorDtoBuilder
                .WithCode(string.Format("{0}{1}", InfrastructureString.ErrorCodeInvalidOperationPrefix, InfrastructureString.ErrorCreateUserCode))
                .WithTitle(InfrastructureString.ErrorCreateUserTitle)
                .WithDetail(InfrastructureString.ErrorCreateUserDetail)
                .WithHttpStatusCode(int.Parse(InfrastructureString.ErrorCreateUserHttpStatusCode))
                .Build();
            ResultDto result = _resultDtoBuilder.WithError(error).Build(); 
            ApplicationUser user =_applicationUserBuilder
                .WithUserName(userName)
                .WithEmail(email)
                .WithPhoneNumber(phoneNumber)
                .Build();

            IdentityResult identityResult = await _userManager.CreateAsync(user, password);
            if (identityResult.Succeeded)
                result = _resultDtoBuilder.BuildSuccess();
            else
                _authLogger.Error("The following errors {@Errors} don't allow delete the user {@user}",
                    identityResult.Errors,
                    user);
            
            return result;
        }

        public async Task<ResultDto> UpdateUserAsync(string userId, string userName, string email, string? phoneNumber)
        {
            var error = _errorDtoBuilder
                .WithCode(string.Format("{0}{1}", InfrastructureString.ErrorCodeInvalidOperationPrefix, InfrastructureString.ErrorUpdateUserCode))
                .WithTitle(InfrastructureString.ErrorUpdateUserTitle)
                .WithDetail(InfrastructureString.ErrorUpdateUserDetail)
                .WithHttpStatusCode(int.Parse(InfrastructureString.ErrorUpdateUserHttpStatusCode))
                .Build();
            ResultDto result = _resultDtoBuilder.WithError(error).Build();

            ApplicationUser? applicationUser = await _userManager.Users.FirstOrDefaultAsync(user => user.Id == userId);

            applicationUser!.UserName = userName;
            applicationUser.Email = email;
            applicationUser.PhoneNumber = phoneNumber;

            IdentityResult identityResult = await _userManager.UpdateAsync(applicationUser);

            if (identityResult.Succeeded)
                result = _resultDtoBuilder.BuildSuccess();
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

            var error = _errorDtoBuilder
                .WithCode(string.Format("{0}{1}", InfrastructureString.ErrorCodeInvalidOperationPrefix, InfrastructureString.ErrorDeleteUserCode))
                .WithTitle(InfrastructureString.ErrorDeleteUserTitle)
                .WithDetail(InfrastructureString.ErrorDeleteUserDetail)
                .WithHttpStatusCode(int.Parse(InfrastructureString.ErrorDeleteUserHttpStatusCode))
                .Build();

            return identityResult.Succeeded
                ? _resultDtoBuilder.BuildSuccess()
                : _resultDtoBuilder.WithError(error).Build();
        }

        public async Task<ResultDto<string?>> AuthenticateMemberAsync(string userName, string password)
        {
            var error = _errorDtoBuilder
                .WithCode(string.Format("{0}{1}", InfrastructureString.ErrorCodeArgumentPrefix, InfrastructureString.ErrorAuthenticateMemberCode))
                .WithTitle(InfrastructureString.ErrorAuthenticateMemberTitle)
                .WithDetail(InfrastructureString.ErrorAuthenticateMemberDetail)
                .WithHttpStatusCode(int.Parse(InfrastructureString.ErrorAuthenticateMemberHttpStatusCode))
                .Build();

            var result = _resultDtoGenericBuilder.BuildFailure<string?>(error);
            var applicationUser = await _userManager.FindByNameAsync(userName);

            if (applicationUser == default || !await _userManager.CheckPasswordAsync(applicationUser, password))
                _authLogger.Warning("Invalid username or password!");
            else
            {
                var roles = await _userManager.GetRolesAsync(applicationUser);
                result = _resultDtoGenericBuilder.BuildSuccess<string?>(JsonSerializer.Serialize(applicationUser.ToUserIdentification(roles?.ToArray())));
            }

            return result;
        }

        public bool DoesUserNameNotExist(string userName)
            => _userManager.Users.All(u => u.UserName != userName);

        public bool IsUserIdAssigned(string userId)
            => _userManager.Users.Any(u => u.Id == userId);
    }
}
