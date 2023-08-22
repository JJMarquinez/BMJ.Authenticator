using BMJ.Authenticator.Application.UseCases.Users.Commands.CreateUser;
using BMJ.Authenticator.Application.UseCases.Users.Commands.DeleteUser;
using BMJ.Authenticator.Application.UseCases.Users.Commands.UpdateUser;
using BMJ.Authenticator.Infrastructure.Events;
using MediatR;

namespace BMJ.Authenticator.Infrastructure.Handlers;

public class EventHandler : IEventHandler
{
    private readonly ISender _mediator;

    public EventHandler(ISender mediator)
    {
        _mediator = mediator;
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
        await _mediator.Send(command);
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
        await _mediator.Send(command);
    }

    public async Task On(UserDeletedEvent @event)
    {
        var command = new DeleteUserCommand
        {
            Id = @event.UserId.ToString()
        };
        await _mediator.Send(command);
    }
}
