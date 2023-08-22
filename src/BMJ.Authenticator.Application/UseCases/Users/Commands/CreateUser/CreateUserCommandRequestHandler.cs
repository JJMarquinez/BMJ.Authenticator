using AutoMapper;
using BMJ.Authenticator.Application.Common.Abstractions;
using BMJ.Authenticator.Application.Common.Models.Results;
using MediatR;

namespace BMJ.Authenticator.Application.UseCases.Users.Commands.CreateUser;

public class CreateUserCommandRequestHandler
    : IRequestHandler<CreateUserCommandRequest, ResultDto<string?>>
{
    private readonly IIdentityAdapter _identityService;
    private readonly IMapper _mapper;

    public CreateUserCommandRequestHandler(IIdentityAdapter identityService, IMapper mapper)
    {
        _identityService = identityService;
        _mapper = mapper;
    }

    public async Task<ResultDto<string?>> Handle(CreateUserCommandRequest request, CancellationToken cancellationToken)
        => await _identityService.CreateUserAsync(request.UserName!, request.Password!, request.Email!, request.PhoneNumber);
}
