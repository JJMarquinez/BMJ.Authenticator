using BMJ.Authenticator.Application.UseCases.Users.Commands.UpdateUser;
using MediatR;
using Microsoft.AspNetCore.OutputCaching;

namespace BMJ.Authenticator.Infrastructure.Events.Handlers.Strategies;

public class UserUpdatedEventHandlerStrategy : EventHandlerStrategy
{
    private readonly ISender _mediator;
    private readonly IOutputCacheStore _cache;

    public UserUpdatedEventHandlerStrategy(ISender mediator, IOutputCacheStore cache)
    {
        _mediator = mediator;
        _cache = cache;
    }

    public override async Task HandlerAsync(BaseEvent @event)
    {
        var userUpdatedEvent = (UserUpdatedEvent) @event;
        var command = new UpdateUserCommand
        {
            Id = userUpdatedEvent.Id.ToString(),
            UserName = userUpdatedEvent.UserName,
            Email = userUpdatedEvent.Email,
            PhoneNumber = userUpdatedEvent.Phone
        };

        var resultDto = await _mediator.Send(command);

        if (resultDto.Success)
        {
            var ct = new CancellationTokenSource().Token;
            await _cache.EvictByTagAsync(command.Id, ct);
            await _cache.EvictByTagAsync("getAllAsync", ct);
        }
    }

    public override bool Support(Type eventType)
        => typeof(UserUpdatedEvent).IsAssignableFrom(eventType);

}
