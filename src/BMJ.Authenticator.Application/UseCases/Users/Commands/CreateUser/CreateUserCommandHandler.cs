using AutoMapper;
using BMJ.Authenticator.Application.Common.Abstractions;
using BMJ.Authenticator.Application.Common.Models.Results;
using MediatR;

namespace BMJ.Authenticator.Application.UseCases.Users.Commands.CreateUser;

public class CreateUserCommandHandler
    : IRequestHandler<CreateUserCommand, ResultDto<string?>>
{
    private readonly IIdentityAdapter _identityService;
    private readonly IMapper _mapper;

    public CreateUserCommandHandler(IIdentityAdapter identityService, IMapper mapper)
    {
        _identityService = identityService;
        _mapper = mapper;
    }

    public async Task<ResultDto<string?>> Handle(CreateUserCommand request, CancellationToken cancellationToken)
        => await _identityService.CreateUserAsync(request.UserName!, request.Password!, request.Email!, request.PhoneNumber);
}
