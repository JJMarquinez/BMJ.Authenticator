using BMJ.Authenticator.Application.Common.Mappings;
using BMJ.Authenticator.Domain.Entities.Users;

namespace BMJ.Authenticator.Application.Common.Models;

public class UserDto : IMapFrom<User>
{
    public string Id { get; set; }
    public string UserName { get; set; }
    public string Email { get; set; }
    public string PhoneNumber { get; set; }
    public string[] Roles { get; set; }
}
