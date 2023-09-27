using BMJ.Authenticator.Application.UseCases.Users.Commands.DeleteUser;
using MediatR;
using Microsoft.AspNetCore.OutputCaching;

namespace BMJ.Authenticator.Infrastructure.Events.Handlers.Strategies;

public class UserDeletedEventHandlerStrategy : EventHandlerStrategy
{
    private readonly ISender _mediator;
    private readonly IOutputCacheStore _cache;

    public UserDeletedEventHandlerStrategy(ISender mediator, IOutputCacheStore cache)
    {
        _mediator = mediator;
        _cache = cache;
    }

    public override async Task HandlerAsync(BaseEvent @event)
    {
        var command = new DeleteUserCommand
        {
            Id = @event.Id.ToString()
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
        => typeof(UserDeletedEvent).IsAssignableFrom(eventType);

}
