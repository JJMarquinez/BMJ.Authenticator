using BMJ.Authenticator.Application.Common.Models.Results;
using BMJ.Authenticator.Application.Common.Models.Results.Builders;
using BMJ.Authenticator.Application.UseCases.Users.Commands.CreateUser.Builders;
using BMJ.Authenticator.Application.UseCases.Users.Commands.DeleteUser.Builders;
using BMJ.Authenticator.Application.UseCases.Users.Commands.UpdateUser.Builders;
using BMJ.Authenticator.Infrastructure.Events;
using BMJ.Authenticator.Infrastructure.Events.Handlers;
using BMJ.Authenticator.Infrastructure.Events.Handlers.Factories;
using BMJ.Authenticator.Infrastructure.Events.Handlers.Strategies;
using MediatR;
using Microsoft.AspNetCore.OutputCaching;
using Moq;

namespace BMJ.Authenticator.Infrastructure.UnitTests.Handlers;

public class EventHandlerTests
{
    private readonly Mock<ISender> _sender;
    private readonly Mock<IOutputCacheStore> _cache;
    private readonly IEventHandlerStrategyContext _eventHandlerStrategyContext;
    private readonly IEventHandlerStrategyFactory _eventHandlerStrategyFactory;
    private readonly IEnumerable<EventHandlerStrategy> _eventHandlerStrategies;
    private readonly ICreateUserCommandBuilder _createUserCommandBuilder;
    private readonly IDeleteUserCommandBuilder _deleteUserCommandBuilder;
    private readonly IUpdateUserCommandBuilder _updateUserCommandBuilder;

    public EventHandlerTests()
    {
        _sender = new();
        _sender.Setup(x => x.Send(
            It.IsAny<IRequest<ResultDto>>(), 
            It.IsAny<CancellationToken>()
            )).ReturnsAsync(new ResultDtoBuilder().BuildSuccess());

        _cache = new();
        _cache.Setup(x => x.EvictByTagAsync(
            It.IsAny<string>(),
            It.IsAny<CancellationToken>()
            )).Returns(new ValueTask());

        _createUserCommandBuilder = new CreateUserCommandBuilder();
        _deleteUserCommandBuilder = new DeleteUserCommandBuilder();
        _updateUserCommandBuilder = new UpdateUserCommandBuilder();

        _eventHandlerStrategies = new List<EventHandlerStrategy> 
        {
            new UserCreatedEventHandlerStrategy(_sender.Object, _cache.Object, _createUserCommandBuilder),
            new UserUpdatedEventHandlerStrategy(_sender.Object, _cache.Object, _updateUserCommandBuilder),
            new UserDeletedEventHandlerStrategy(_sender.Object, _cache.Object, _deleteUserCommandBuilder)
        };
        _eventHandlerStrategyFactory = new EventHandlerStrategyFactory(_eventHandlerStrategies);
        _eventHandlerStrategyContext = new EventHandlerStrategyContext(_eventHandlerStrategyFactory);
    }

    [Fact]
    public async void ShouldHandleUserCreatedEvent()
    {
        var @event = new UserCreatedEvent
        {
            UserName = "Joe",
            Email = "joe@authenticator.com",
            Phone = "111-222-333",
            Password = "22*kZ7V8ISl$",
            Version = 9
        };
        var exception = await Record.ExceptionAsync(() => _eventHandlerStrategyContext.ExecuteHandlingAsync(@event));
        Assert.Null(exception);
    }

    [Fact]
    public async void ShouldHandleUserUpdatedEvent()
    {
        var @event = new UserUpdatedEvent
        {
            UserName = "Joe",
            Email = "joe@authenticator.com",
            Phone = "111-222-333",
            Version = 9
        };
        var exception = await Record.ExceptionAsync(() => _eventHandlerStrategyContext.ExecuteHandlingAsync(@event));
        Assert.Null(exception);
    }

    [Fact]
    public async void ShouldHandleUserDeletedEvent()
    {
        var @event = new UserDeletedEvent
        {
            Id = Guid.NewGuid(),
            Version = 9
        };
        var exception = await Record.ExceptionAsync(() => _eventHandlerStrategyContext.ExecuteHandlingAsync(@event));
        Assert.Null(exception);
    }
}
