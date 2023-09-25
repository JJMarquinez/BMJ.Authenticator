namespace BMJ.Authenticator.Infrastructure.Identity;

public struct UserIdentification
{
    public string Id { get; set; }
    public string UserName { get; set; }
    public string Email { get; set; }
    public string PhoneNumber { get; set; }
    public string[] Roles { get; set; }
}
