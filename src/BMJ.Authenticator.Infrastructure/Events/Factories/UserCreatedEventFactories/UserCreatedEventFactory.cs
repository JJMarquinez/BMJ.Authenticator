using BMJ.Authenticator.Infrastructure.Events.Factories.UserCreatedEventFactories.Contexts;

namespace BMJ.Authenticator.Infrastructure.Events.Factories.UserCreatedEventFactories;

public class UserCreatedEventFactory : EventFactory
{
    public override BaseEvent Make(BaseEventContext context)
    {
        var userCreatedEventContext = (UserCreatedEventContext)context;
        return new UserCreatedEvent
        {
            Id = userCreatedEventContext.Id,
            Type = userCreatedEventContext.Type,
            Version = userCreatedEventContext.Version,
            UserName = userCreatedEventContext.UserName,
            Email = userCreatedEventContext.Email,
            Phone = userCreatedEventContext.Phone,
            Password = userCreatedEventContext.Password
        };
    }

    public override bool Support(Type type)
        => typeof(UserCreatedEventContext).IsAssignableFrom(type);
}
