using BMJ.Authenticator.Infrastructure.Events.Factories.UserDeletedEventFactories.Contexts;

namespace BMJ.Authenticator.Infrastructure.Events.Factories.UserDeletedEventFactories;

public class UserDeletedEventFactory : EventFactory
{
    public override BaseEvent Make(BaseEventContext context)
    {
        var userDeletedEventContext = (UserDeletedEventContext)context;
        return new UserDeletedEvent
        { 
            Id = userDeletedEventContext.Id,
            Type = userDeletedEventContext.Type,
            Version = userDeletedEventContext.Version
        };
    }

    public override bool Support(Type type)
        => typeof(UserDeletedEventContext).IsAssignableFrom(type);
}
