using BMJ.Authenticator.Domain.Entities.Users;
using BMJ.Authenticator.Domain.ValueObjects;

namespace BMJ.Authenticator.Application.Common.Models.Users;

public static class UserDtoExtensions
{
    public static User ToUser(this UserDto userDto)
    {
        var userBuilder = User.Builder()
            .WithId(userDto.Id)
            .WithName(userDto.UserName)
            .WithEmail((Email)userDto.Email)
            .WithRoles(userDto.Roles);

        if (userDto.PhoneNumber is not null)
            userBuilder.WithPhone((Phone)userDto.PhoneNumber);

        return userBuilder.Build();
    }
}
