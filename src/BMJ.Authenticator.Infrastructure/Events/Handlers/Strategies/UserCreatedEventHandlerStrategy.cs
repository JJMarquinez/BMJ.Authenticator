using BMJ.Authenticator.Application.UseCases.Users.Commands.CreateUser.Builders;
using MediatR;
using Microsoft.AspNetCore.OutputCaching;

namespace BMJ.Authenticator.Infrastructure.Events.Handlers.Strategies;

public class UserCreatedEventHandlerStrategy : EventHandlerStrategy
{
    private readonly ISender _mediator;
    private readonly IOutputCacheStore _cache;
    private readonly ICreateUserCommandBuilder _createUserCommandBuilder;

    public UserCreatedEventHandlerStrategy(ISender mediator, IOutputCacheStore cache, ICreateUserCommandBuilder createUserCommandBuilder)
    {
        _mediator = mediator;
        _cache = cache;
        _createUserCommandBuilder = createUserCommandBuilder;
    }

    public override async Task HandlerAsync(BaseEvent @event)
    {
        var userCreatedEvent = (UserCreatedEvent)@event;
        var command = _createUserCommandBuilder
            .WithUsername(userCreatedEvent.UserName)
            .WithEmail(userCreatedEvent.Email)
            .WithPhoneNumber(userCreatedEvent.Phone)
            .WithPassword(userCreatedEvent.Password)
            .Build();

        var resultDto = await _mediator.Send(command);

        if (resultDto.Success)
            await _cache.EvictByTagAsync("getAllAsync", new CancellationTokenSource().Token);
    }

    public override bool Support(Type eventType)
        => typeof(UserCreatedEvent).IsAssignableFrom(eventType);
}
