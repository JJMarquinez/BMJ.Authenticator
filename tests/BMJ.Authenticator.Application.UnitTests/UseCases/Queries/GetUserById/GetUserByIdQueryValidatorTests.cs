using BMJ.Authenticator.Application.Common.Abstractions;
using BMJ.Authenticator.Application.UseCases.Users.Queries.GetUserById;
using BMJ.Authenticator.Application.UseCases.Users.Queries.GetUserById.Factories;
using FluentValidation;
using Moq;

namespace BMJ.Authenticator.Application.UnitTests.UseCases.Queries.GetUserById;

public class GetUserByIdQueryValidatorTests
{
    private readonly Mock<IIdentityAdapter> _identityAdapter;
    private readonly IGetUserByIdQueryFactory _getUserByIdQueryFactory;

    public GetUserByIdQueryValidatorTests()
    {
        _identityAdapter = new();
        _getUserByIdQueryFactory = new GetUserByIdQueryFactory();
    }

    [Fact]
    public async void ShouldValidateGetUserByIdQueryRequest()
    {
        _identityAdapter.Setup(x => x.IsUserIdAssigned(It.IsAny<string>())).Returns(true);
        AbstractValidator<GetUserByIdQuery> validator = new GetUserByIdQueryValidator(_identityAdapter.Object);
        var query = _getUserByIdQueryFactory.Genarate(Guid.NewGuid().ToString());
        var token = new CancellationTokenSource().Token;

        var result = await validator.ValidateAsync((GetUserByIdQuery)query, token);

        Assert.True(result.IsValid);
    }

    [Fact]
    public async void ShouldNotValidateGetUserByIdQueryRequestGivenIdAlreadyAssigned()
    {
        _identityAdapter.Setup(x => x.IsUserIdAssigned(It.IsAny<string>())).Returns(false);
        AbstractValidator<GetUserByIdQuery> validator = new GetUserByIdQueryValidator(_identityAdapter.Object);
        var query = _getUserByIdQueryFactory.Genarate(Guid.NewGuid().ToString());
        var token = new CancellationTokenSource().Token;

        var result = await validator.ValidateAsync((GetUserByIdQuery)query, token);

        Assert.False(result.IsValid);
        Assert.Equal("It doesn't exist any user with the Id sent.", result.Errors[0].ErrorMessage);
    }

    [Fact]
    public async void ShouldNotValidateGetUserByIdQueryRequestGivenNullId()
    {
        _identityAdapter.Setup(x => x.IsUserIdAssigned(It.IsAny<string>())).Returns(false);
        AbstractValidator<GetUserByIdQuery> validator = new GetUserByIdQueryValidator(_identityAdapter.Object);
        var query = _getUserByIdQueryFactory.Genarate(null!);
        var token = new CancellationTokenSource().Token;

        var result = await validator.ValidateAsync((GetUserByIdQuery)query, token);

        Assert.False(result.IsValid);
        Assert.Equal("The Id is mandatory to look for the user.", result.Errors[0].ErrorMessage);
    }
}
