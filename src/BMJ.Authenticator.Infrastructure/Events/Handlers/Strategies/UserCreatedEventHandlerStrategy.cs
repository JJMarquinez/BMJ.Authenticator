using BMJ.Authenticator.Application.UseCases.Users.Commands.CreateUser;
using MediatR;
using Microsoft.AspNetCore.OutputCaching;

namespace BMJ.Authenticator.Infrastructure.Events.Handlers.Strategies;

public class UserCreatedEventHandlerStrategy : EventHandlerStrategy
{
    private readonly ISender _mediator;
    private readonly IOutputCacheStore _cache;

    public UserCreatedEventHandlerStrategy(ISender mediator, IOutputCacheStore cache)
    {
        _mediator = mediator;
        _cache = cache;
    }

    public override async Task HandlerAsync(BaseEvent @event)
    {
        var userCreatedEvent = (UserCreatedEvent)@event;
        var command = new CreateUserCommand
        {
            UserName = userCreatedEvent.UserName,
            Email = userCreatedEvent.Email,
            PhoneNumber = userCreatedEvent.Phone,
            Password = userCreatedEvent.Password
        };

        var resultDto = await _mediator.Send(command);

        if (resultDto.Success)
            await _cache.EvictByTagAsync("getAllAsync", new CancellationTokenSource().Token);
    }

    public override bool Support(Type eventType)
        => typeof(UserCreatedEvent).IsAssignableFrom(eventType);
}
