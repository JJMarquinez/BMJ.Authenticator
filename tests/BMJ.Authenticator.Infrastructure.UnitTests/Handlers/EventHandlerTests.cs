using BMJ.Authenticator.Application.Common.Models.Results;
using BMJ.Authenticator.Infrastructure.Events;
using BMJ.Authenticator.Infrastructure.Handlers;
using MediatR;
using Microsoft.AspNetCore.OutputCaching;
using Moq;

namespace BMJ.Authenticator.Infrastructure.UnitTests.Handlers;

public class EventHandlerTests
{
    private readonly Mock<ISender> _sender;
    private readonly Mock<IOutputCacheStore> _cache;
    private readonly IEventHandler _eventHandler;

    public EventHandlerTests()
    {
        _sender = new();
        _sender.Setup(x => x.Send(
            It.IsAny<IRequest<ResultDto>>(), 
            It.IsAny<CancellationToken>()
            )).ReturnsAsync(ResultDto.NewSuccess);

        _cache = new();
        _cache.Setup(x => x.EvictByTagAsync(
            It.IsAny<string>(),
            It.IsAny<CancellationToken>()
            )).Returns(new ValueTask());

        _eventHandler = new Infrastructure.Handlers.EventHandler(_sender.Object, _cache.Object);
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
        var exception = await Record.ExceptionAsync(() => _eventHandler.On(@event));
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
        var exception = await Record.ExceptionAsync(() => _eventHandler.On(@event));
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
        var exception = await Record.ExceptionAsync(() => _eventHandler.On(@event));
        Assert.Null(exception);
    }
}
