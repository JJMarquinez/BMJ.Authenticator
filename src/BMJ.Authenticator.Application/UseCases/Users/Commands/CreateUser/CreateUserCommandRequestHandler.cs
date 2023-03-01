using AutoMapper;
using BMJ.Authenticator.Application.Common.Abstractions;
using BMJ.Authenticator.Application.Common.Interfaces;
using BMJ.Authenticator.Application.Common.Models.Results;
using BMJ.Authenticator.Domain.Common.Results;
using BMJ.Authenticator.Domain.Entities.Users;
using MediatR;

namespace BMJ.Authenticator.Application.UseCases.Users.Commands.CreateUser;

public class CreateUserCommandRequestHandler
    : IRequestHandler<CreateUserCommandRequest, ResultDto<string?>>
{
    private readonly IIdentityService _identityService;
    private readonly IMapper _mapper;

    public CreateUserCommandRequestHandler(IIdentityService identityService, IMapper mapper)
    {
        _identityService = identityService;
        _mapper = mapper;
    }

    public async Task<ResultDto<string?>> Handle(CreateUserCommandRequest request, CancellationToken cancellationToken)
    {
        Result<string?> userResult = await _identityService.CreateUserAsync(request.UserName, request.Password, request.Email, request.PhoneNumber);
        return _mapper.Map<ResultDto<string?>>(userResult);
    }
}
