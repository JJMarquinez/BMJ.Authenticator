using BMJ.Authenticator.Application.Common.Models.Results;
using MediatR;

namespace BMJ.Authenticator.Application.UseCases.Users.Commands.CreateUser.Builders;

public class CreateUserCommandBuilder : ICreateUserCommandBuilder
{
    private string _email;
    private string _password;
    private string _phoneNumber;
    private string _username;
    public IRequest<ResultDto> Build()
        => new CreateUserCommand
        { 
            Email = _email,
            Password = _password,
            PhoneNumber = _phoneNumber,
            UserName = _username
        };

    public ICreateUserCommandBuilder WithEmail(string email)
    {
        _email = email;
        return this;
    }

    public ICreateUserCommandBuilder WithPassword(string password)
    {
        _password = password;
        return this;
    }

    public ICreateUserCommandBuilder WithPhoneNumber(string phoneNumber)
    {
        _phoneNumber = phoneNumber;
        return this;
    }

    public ICreateUserCommandBuilder WithUsername(string username)
    {
        _username = username;
        return this;
    }
}
