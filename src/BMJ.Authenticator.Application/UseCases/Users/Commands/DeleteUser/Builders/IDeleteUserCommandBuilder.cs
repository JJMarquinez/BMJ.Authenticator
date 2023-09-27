using BMJ.Authenticator.Application.Common.Models.Results;
using MediatR;

namespace BMJ.Authenticator.Application.UseCases.Users.Commands.DeleteUser.Builders;

public interface IDeleteUserCommandBuilder
{
    IDeleteUserCommandBuilder WithId(string? id);
    IRequest<ResultDto> Build();
}
