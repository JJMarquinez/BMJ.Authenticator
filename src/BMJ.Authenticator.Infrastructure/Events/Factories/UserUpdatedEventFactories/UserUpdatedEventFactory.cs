using BMJ.Authenticator.Infrastructure.Events.Factories.UserUpdatedEventFactories.Contexts;

namespace BMJ.Authenticator.Infrastructure.Events.Factories.UserUpdatedEventFactories;

public class UserUpdatedEventFactory : EventFactory
{
    public override BaseEvent Make(BaseEventContext context)
    {
        var userCreatedEventContext = (UserUpdatedEventContext)context;
        return new UserUpdatedEvent
        {
            Id = userCreatedEventContext.Id,
            Type = userCreatedEventContext.Type,
            Version = userCreatedEventContext.Version,
            UserName = userCreatedEventContext.UserName,
            Email = userCreatedEventContext.Email,
            Phone = userCreatedEventContext.Phone
        };
    }

    public override bool Support(Type type)
        => typeof(UserUpdatedEventContext).IsAssignableFrom(type);
}
