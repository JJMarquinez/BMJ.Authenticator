using BMJ.Authenticator.Application.UseCases.Users.Commands.UpdateUser.Builders;
using MediatR;
using Microsoft.AspNetCore.OutputCaching;

namespace BMJ.Authenticator.Infrastructure.Events.Handlers.Strategies;

public class UserUpdatedEventHandlerStrategy : EventHandlerStrategy
{
    private readonly ISender _mediator;
    private readonly IOutputCacheStore _cache;
    private readonly IUpdateUserCommandBuilder _updateUserCommandBuilder;

    public UserUpdatedEventHandlerStrategy(ISender mediator, IOutputCacheStore cache, IUpdateUserCommandBuilder updateUserCommandBuilder)
    {
        _mediator = mediator;
        _cache = cache;
        _updateUserCommandBuilder = updateUserCommandBuilder;
    }

    public override async Task HandlerAsync(BaseEvent @event)
    {
        var userUpdatedEvent = (UserUpdatedEvent) @event;
        var userId = userUpdatedEvent.Id.ToString();
        var command = _updateUserCommandBuilder
            .WithId(userId)
            .WithUsername(userUpdatedEvent.UserName)
            .WithEmail(userUpdatedEvent.Email)
            .WithPhoneNumber(userUpdatedEvent.Phone)
            .Build();

        var resultDto = await _mediator.Send(command);

        if (resultDto.Success)
        {
            var ct = new CancellationTokenSource().Token;
            await _cache.EvictByTagAsync(userId, ct);
            await _cache.EvictByTagAsync("getAllAsync", ct);
        }
    }

    public override bool Support(Type eventType)
        => typeof(UserUpdatedEvent).IsAssignableFrom(eventType);

}
