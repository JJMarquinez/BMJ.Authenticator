using BMJ.Authenticator.Application.Common.Models.Results;
using MediatR;

namespace BMJ.Authenticator.Application.UseCases.Users.Commands.UpdateUser.Builders;

public class UpdateUserCommandBuilder : IUpdateUserCommandBuilder
{
    private string _id;
    private string _email;
    private string _phoneNumber;
    private string _username;

    public IRequest<ResultDto> Build()
        => new UpdateUserCommand
        { 
            Id = _id,
            Email = _email,
            PhoneNumber = _phoneNumber,
            UserName = _username
        };

    public IUpdateUserCommandBuilder WithEmail(string? email)
    {
        _email = email;
        return this;
    }

    public IUpdateUserCommandBuilder WithId(string? id)
    {
        _id = id;
        return this;
    }

    public IUpdateUserCommandBuilder WithPhoneNumber(string? phoneNumber)
    {
        _phoneNumber = phoneNumber;
        return this;
    }

    public IUpdateUserCommandBuilder WithUsername(string? username)
    {
        _username = username;
        return this;
    }
}
