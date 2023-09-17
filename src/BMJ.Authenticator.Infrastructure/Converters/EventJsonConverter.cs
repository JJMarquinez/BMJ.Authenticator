using BMJ.Authenticator.Infrastructure.Events;
using System.Text.Json.Serialization;
using System.Text.Json;

namespace BMJ.Authenticator.Infrastructure.Converters;

public class EventJsonConverter : JsonConverter<BaseEvent>
{
    public override bool CanConvert(Type typeToConvert)
    {
        return typeToConvert.IsAssignableFrom(typeof(BaseEvent));
    }
    public override BaseEvent? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        var doc = JsonDocument.ParseValue(ref reader);

        if (!doc.RootElement.TryGetProperty("Type", out var type))
            throw new JsonException("Could not detect the Type discriminator property!");

        var typeDiscriminator = type.GetString();
        var json = doc.RootElement.GetRawText();

        return typeDiscriminator switch
        {
            nameof(UserCreatedEvent) => JsonSerializer.Deserialize<UserCreatedEvent>(json, options),
            nameof(UserUpdatedEvent) => JsonSerializer.Deserialize<UserUpdatedEvent>(json, options),
            nameof(UserDeletedEvent) => JsonSerializer.Deserialize<UserDeletedEvent>(json, options),
            _ => throw new JsonException($"{typeDiscriminator} is not supported yet!")
        };
    }

    public override void Write(Utf8JsonWriter writer, BaseEvent value, JsonSerializerOptions options)
    {
        throw new NotImplementedException();
    }
}
