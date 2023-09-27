using BMJ.Authenticator.Application.Common.Models.Results;
using MediatR;

namespace BMJ.Authenticator.Application.UseCases.Users.Commands.DeleteUser.Builders;

public class DeleteUserCommandBuilder : IDeleteUserCommandBuilder
{
    private string? _id;
    public IRequest<ResultDto> Build()
        => new DeleteUserCommand { Id = _id };

    public IDeleteUserCommandBuilder WithId(string? id)
    {
        _id = id;
        return this;
    }
}
