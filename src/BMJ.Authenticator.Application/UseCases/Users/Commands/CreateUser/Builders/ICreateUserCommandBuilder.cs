using BMJ.Authenticator.Application.Common.Models.Results;
using MediatR;

namespace BMJ.Authenticator.Application.UseCases.Users.Commands.CreateUser.Builders;

public interface ICreateUserCommandBuilder
{
    ICreateUserCommandBuilder WithUsername(string? username);
    ICreateUserCommandBuilder WithEmail(string? email);
    ICreateUserCommandBuilder WithPhoneNumber(string? phoneNumber);
    ICreateUserCommandBuilder WithPassword(string? password);
    IRequest<ResultDto> Build();
}
