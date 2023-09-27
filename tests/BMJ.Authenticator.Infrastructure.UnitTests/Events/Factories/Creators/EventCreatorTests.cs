using BMJ.Authenticator.Infrastructure.Events;
using BMJ.Authenticator.Infrastructure.Events.Factories;
using BMJ.Authenticator.Infrastructure.Events.Factories.Creators;
using BMJ.Authenticator.Infrastructure.Events.Factories.UserCreatedEventFactories;
using BMJ.Authenticator.Infrastructure.Events.Factories.UserCreatedEventFactories.Contexts.Builders;
using BMJ.Authenticator.Infrastructure.Events.Factories.UserDeletedEventFactories;
using BMJ.Authenticator.Infrastructure.Events.Factories.UserDeletedEventFactories.Contexts.Builders;
using BMJ.Authenticator.Infrastructure.Events.Factories.UserUpdatedEventFactories;
using BMJ.Authenticator.Infrastructure.Events.Factories.UserUpdatedEventFactories.Contexts.Builders;
using BMJ.Authenticator.Infrastructure.Properties;

namespace BMJ.Authenticator.Infrastructure.UnitTests.Events.Factories.Creators;

public class EventCreatorTests
{
    private readonly IEventCreator _eventCreator;
    private readonly IEnumerable<EventFactory> _eventFactories;

    public EventCreatorTests()
    {
        _eventFactories = new List<EventFactory>
        {
            new UserDeletedEventFactory(),
            new UserCreatedEventFactory(),
            new UserUpdatedEventFactory()
        };
        _eventCreator = new EventCreator(_eventFactories);
    }

    [Fact]
    public void SouldCreateUserDeletedEvent()
    {
        IUserDeletedEventContextBuilder eventContextBuilder = new UserDeletedEventContextBuilder();
        var eventContext = eventContextBuilder
            .WithId(Guid.NewGuid())
            .WithVersion(9)
            .Build();

        var @event = _eventCreator.Create(eventContext);
        
        Assert.NotNull(@event);
        Assert.IsType<UserDeletedEvent>(@event);
        Assert.Equal(eventContext.Id, @event.Id);
        Assert.Equal(eventContext.Version, @event.Version);
        Assert.Equal(eventContext.Type, @event.Type);
    }

    [Fact]
    public void SouldCreateUserCreatedEvent()
    {
        IUserCreatedEventContextBuilder eventContextBuilder = new UserCreatedEventContextBuilder();
        var eventContext = eventContextBuilder
            .WithId(Guid.NewGuid())
            .WithVersion(9)
            .WithUsername("Silly")
            .WithEmail("silly@authenticator.com")
            .WithPhone("111-222-333")
            .WithPassword("=WC£1%/Hu9!6")
            .Build();

        var @event = _eventCreator.Create(eventContext);

        Assert.NotNull(@event);
        Assert.IsType<UserCreatedEvent>(@event);
        Assert.Equal(eventContext.Id, @event.Id);
        Assert.Equal(eventContext.Version, @event.Version);
        Assert.Equal(eventContext.Type, @event.Type);
        Assert.Equal(eventContext.UserName, ((UserCreatedEvent)@event).UserName);
        Assert.Equal(eventContext.Email, ((UserCreatedEvent)@event).Email);
        Assert.Equal(eventContext.Phone, ((UserCreatedEvent)@event).Phone);
        Assert.Equal(eventContext.Password, ((UserCreatedEvent)@event).Password);
    }

    [Fact]
    public void SouldCreateUserUpdatedEvent()
    {
        IUserUpdatedEventContextBuilder eventContextBuilder = new UserUpdatedEventContextBuilder();
        var eventContext = eventContextBuilder
            .WithId(Guid.NewGuid())
            .WithVersion(9)
            .WithUsername("Silly")
            .WithEmail("silly@authenticator.com")
            .WithPhone("111-222-333")
            .Build();

        var @event = _eventCreator.Create(eventContext);

        Assert.NotNull(@event);
        Assert.IsType<UserUpdatedEvent>(@event);
        Assert.Equal(eventContext.Id, @event.Id);
        Assert.Equal(eventContext.Version, @event.Version);
        Assert.Equal(eventContext.Type, @event.Type);
        Assert.Equal(eventContext.UserName, ((UserUpdatedEvent)@event).UserName);
        Assert.Equal(eventContext.Email, ((UserUpdatedEvent)@event).Email);
        Assert.Equal(eventContext.Phone, ((UserUpdatedEvent)@event).Phone);
    }

    [Fact]
    public void ShouldThrowExceptionGivenUnRegisterUserDeletedEventFactory()
    {
        var eventFactories = new List<EventFactory>
        {
            new UserCreatedEventFactory(),
            new UserUpdatedEventFactory()
        };
        IEventCreator eventCreator = new EventCreator(eventFactories);
        IUserDeletedEventContextBuilder eventContextBuilder = new UserDeletedEventContextBuilder();
        var eventContext = eventContextBuilder
            .WithId(Guid.NewGuid())
            .WithVersion(9)
            .Build();

        var exception = Assert.Throws<Exception>(() => eventCreator.Create(eventContext));

        Assert.Equal(InfrastructureString.AmbiguousOrNoOneEventFactory, exception.Message);
    }
}
