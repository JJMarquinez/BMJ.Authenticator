using BMJ.Authenticator.Application.UseCases.Users.Commands.CreateUser;
using BMJ.Authenticator.Application.UseCases.Users.Commands.DeleteUser;
using BMJ.Authenticator.Application.UseCases.Users.Commands.UpdateUser;
using BMJ.Authenticator.Infrastructure.Events;
using MediatR;
using Microsoft.AspNetCore.OutputCaching;

namespace BMJ.Authenticator.Infrastructure.Handlers;

public class EventHandler : IEventHandler
{
    private readonly ISender _mediator;
    private readonly IOutputCacheStore _cache;

    public EventHandler(ISender mediator, IOutputCacheStore cache)
    {
        _mediator = mediator;
        _cache = cache;
    }

    public async Task On(UserCreatedEvent @event)
    {
        var command = new CreateUserCommand
        { 
            UserName = @event.UserName,
            Email = @event.Email,
            PhoneNumber = @event.Phone,
            Password = @event.Password
        };

        var resultDto = await _mediator.Send(command);

        if (resultDto.Success)
            await _cache.EvictByTagAsync("getAllAsync", new CancellationTokenSource().Token);
    }

    public async Task On(UserUpdatedEvent @event)
    {
        var command = new UpdateUserCommand
        {
            Id = @event.UserId.ToString(),
            UserName = @event.UserName,
            Email = @event.Email,
            PhoneNumber = @event.Phone
        };

        var resultDto = await _mediator.Send(command);

        if (resultDto.Success)
        {
            var ct = new CancellationTokenSource().Token;
            await _cache.EvictByTagAsync(command.Id, ct);
            await _cache.EvictByTagAsync("getAllAsync", ct); 
        }
    }

    public async Task On(UserDeletedEvent @event)
    {
        var command = new DeleteUserCommand
        {
            Id = @event.UserId.ToString()
        };

        var resultDto = await _mediator.Send(command);

        if (resultDto.Success)
        {
            var ct = new CancellationTokenSource().Token;
            await _cache.EvictByTagAsync(command.Id, ct);
            await _cache.EvictByTagAsync("getAllAsync", ct);
        }
    }
}
