using BMJ.Authenticator.Application.Common.Abstractions;
using BMJ.Authenticator.Application.Common.Models;
using BMJ.Authenticator.Domain.Entities.Users;
using BMJ.Authenticator.Domain.ValueObjects;
using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;

namespace BMJ.Authenticator.Infrastructure.Identity
{
    public static class ApplicationUserExtensions
    {
        public static User ToApplicationUser(this ApplicationUser applicationUser, string[]? roles)
            => User.New(
                applicationUser.Id, 
                applicationUser.UserName,
                Email.From(applicationUser.Email), 
                roles,
                applicationUser.PhoneNumber is not null ? Phone.New(applicationUser.PhoneNumber) : null,
                applicationUser.PasswordHash);
    }
}
