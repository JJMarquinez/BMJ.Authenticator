using BMJ.Authenticator.Infrastructure.Converters;
using BMJ.Authenticator.Infrastructure.Events;
using System.Text.Json;

namespace BMJ.Authenticator.Infrastructure.UnitTests.Converters;

public class EventJsonConverterTests
{
    private readonly string _userName = "";
    private readonly string _email = "";
    private readonly string _phone = "";
    private readonly string _password = "";
    private readonly int _version;
    public EventJsonConverterTests()
    {
        _userName = "Joe";
        _email = "joe@authenticator.com";
        _phone = "111-222-333";
        _password = "22*kZ7V8ISl$";
        _version = 9;
    }

    [Fact]
    public void ShouldDeserializeUserCreatedEvent()
    {
        var eventToSerialize = JsonSerializer.Serialize(
            new UserCreatedEvent
            {
                UserName = _userName,
                Email = _email,
                Phone = _phone,
                Password = _password,
                Version = _version
            }
        );

        var options = new JsonSerializerOptions { Converters = { new EventJsonConverter() } };
        var @event = JsonSerializer.Deserialize<BaseEvent>(eventToSerialize, options)!;

        Assert.IsAssignableFrom<BaseEvent>(@event);
        Assert.Equal(_userName, ((UserCreatedEvent)@event).UserName);
        Assert.Equal(_email, ((UserCreatedEvent)@event).Email);
        Assert.Equal(_phone, ((UserCreatedEvent)@event).Phone);
        Assert.Equal(_password, ((UserCreatedEvent)@event).Password);
        Assert.Equal(nameof(UserCreatedEvent), ((UserCreatedEvent)@event).Type);
        Assert.Equal(_version, ((UserCreatedEvent)@event).Version);
        Assert.Equal(Guid.Empty, ((UserCreatedEvent)@event).Id);
    }

    [Fact]
    public void ShouldDeserializeUserUpdatedEvent()
    {
        var eventToSerialize = JsonSerializer.Serialize(
            new UserUpdatedEvent
            {
                UserName = _userName,
                Email = _email,
                Phone = _phone,
                Version = _version
            }
        );

        var options = new JsonSerializerOptions { Converters = { new EventJsonConverter() } };
        var @event = JsonSerializer.Deserialize<BaseEvent>(eventToSerialize, options)!;

        Assert.IsAssignableFrom<BaseEvent>(@event);
        Assert.Equal(_userName, ((UserUpdatedEvent)@event).UserName);
        Assert.Equal(_email, ((UserUpdatedEvent)@event).Email);
        Assert.Equal(_phone, ((UserUpdatedEvent)@event).Phone);
        Assert.Equal(nameof(UserUpdatedEvent), ((UserUpdatedEvent)@event).Type);
        Assert.Equal(_version, ((UserUpdatedEvent)@event).Version);
        Assert.Equal(Guid.Empty, ((UserUpdatedEvent)@event).Id);
    }

    [Fact]
    public void ShouldDeserializeUserDeletedEvent()
    {
        var id = Guid.NewGuid();
        var eventToSerialize = JsonSerializer.Serialize(
            new UserDeletedEvent
            {
                Id = id,
                Version = _version
            }
        );

        var options = new JsonSerializerOptions { Converters = { new EventJsonConverter() } };
        var @event = JsonSerializer.Deserialize<BaseEvent>(eventToSerialize, options)!;

        Assert.IsAssignableFrom<BaseEvent>(@event);
        Assert.Equal(id, ((UserDeletedEvent)@event).Id);
        Assert.Equal(_version, ((UserDeletedEvent)@event).Version);
        Assert.Equal(nameof(UserDeletedEvent), ((UserDeletedEvent)@event).Type);
    }

    [Fact]
    public void ShouldThrowJsonExceptionGivenWrongJson()
    {
        var wrongJson = "\"UserId\":\"00000000-0000-0000-0000-000000000000\",\"Version\":9,\"Type\":\"UserDeletedEvent\",\"Id\":\"69e93ef8-cf4d-4ed1-aa1e-29674634e3ac\"}";

        var options = new JsonSerializerOptions { Converters = { new EventJsonConverter() } };
        Assert.Throws<JsonException>( () => JsonSerializer.Deserialize<BaseEvent>(wrongJson, options));
    }

    [Fact]
    public void ShouldThrowJsonExceptionGivenNoTypeProperty()
    {
        var wrongJson = "{\"UserId\":\"00000000-0000-0000-0000-000000000000\",\"Version\":9,\"Id\":\"69e93ef8-cf4d-4ed1-aa1e-29674634e3ac\"}";

        var options = new JsonSerializerOptions { Converters = { new EventJsonConverter() } };
        var exception = Assert.Throws<JsonException>(() => JsonSerializer.Deserialize<BaseEvent>(wrongJson, options));

        Assert.Equal("Could not detect the Type discriminator property!", exception.Message);
    }

    [Fact]
    public void ShouldThrowJsonExceptionGivenUnsopportedEventType()
    {
        var wrongJson = "{\"UserId\":\"00000000-0000-0000-0000-000000000000\",\"Version\":9,\"Type\":\"UserNonExistingEvent\",\"Id\":\"69e93ef8-cf4d-4ed1-aa1e-29674634e3ac\"}";

        var options = new JsonSerializerOptions { Converters = { new EventJsonConverter() } };
        var exception = Assert.Throws<JsonException>(() => JsonSerializer.Deserialize<BaseEvent>(wrongJson, options));

        Assert.Equal($"UserNonExistingEvent is not supported yet!", exception.Message);
    }
}
