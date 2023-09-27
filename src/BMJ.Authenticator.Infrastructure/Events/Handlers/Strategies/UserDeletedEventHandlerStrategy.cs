using BMJ.Authenticator.Application.UseCases.Users.Commands.DeleteUser.Builders;
using MediatR;
using Microsoft.AspNetCore.OutputCaching;

namespace BMJ.Authenticator.Infrastructure.Events.Handlers.Strategies;

public class UserDeletedEventHandlerStrategy : EventHandlerStrategy
{
    private readonly ISender _mediator;
    private readonly IOutputCacheStore _cache;
    private readonly IDeleteUserCommandBuilder _deleteUserCommandBuilder;

    public UserDeletedEventHandlerStrategy(ISender mediator, IOutputCacheStore cache, IDeleteUserCommandBuilder deleteUserCommandBuilder)
    {
        _mediator = mediator;
        _cache = cache;
        _deleteUserCommandBuilder = deleteUserCommandBuilder;
    }

    public override async Task HandlerAsync(BaseEvent @event)
    {
        var userId = @event.Id.ToString();
        var command = _deleteUserCommandBuilder.WithId(userId).Build();

        var resultDto = await _mediator.Send(command);

        if (resultDto.Success)
        {
            var ct = new CancellationTokenSource().Token;
            await _cache.EvictByTagAsync(userId, ct);
            await _cache.EvictByTagAsync("getAllAsync", ct);
        }
    }

    public override bool Support(Type eventType)
        => typeof(UserDeletedEvent).IsAssignableFrom(eventType);

}
