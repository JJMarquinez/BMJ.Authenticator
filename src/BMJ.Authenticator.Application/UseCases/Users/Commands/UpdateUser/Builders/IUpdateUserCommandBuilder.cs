using BMJ.Authenticator.Application.Common.Models.Results;
using MediatR;

namespace BMJ.Authenticator.Application.UseCases.Users.Commands.UpdateUser.Builders;

public interface IUpdateUserCommandBuilder
{
    IUpdateUserCommandBuilder WithId(string? id);
    IUpdateUserCommandBuilder WithUsername(string? username);
    IUpdateUserCommandBuilder WithEmail(string? email);
    IUpdateUserCommandBuilder WithPhoneNumber(string? phoneNumber);
    IRequest<ResultDto> Build();
}
