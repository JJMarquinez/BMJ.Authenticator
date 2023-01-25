using BMJ.Authenticator.Domain;

namespace BMJ.Authenticator.Application.Common.Models
{
    public class User
    {
        private string id;
        private string userName;
        private string[] roles;

        private User(string id, string userName, string[] roles) 
        {
            Ensure.Argument.NotNullOrEmpty(id, nameof(id));
            Ensure.Argument.NotNullOrEmpty(userName, nameof(userName));
            Ensure.Argument.Is(roles != null && roles.Length >= 1, nameof(roles));
            this.id = id;
            this.userName = userName;
            this.roles = roles;
        }

        public static User New(string id, string userName, string[] roles)
            => new(id, userName, roles);

        public string GetId() => id;
        public string GetUserName() => userName;
        public string[] GetRoles() => roles;
        public bool IsInRole(string role) => roles.Contains(role);
    }
}
