using AutoMapper;
using BMJ.Authenticator.Application.Common.Instrumentation;
using BMJ.Authenticator.Application.Common.Abstractions;
using BMJ.Authenticator.Application.Common.Models.Results;
using MediatR;
using System.Diagnostics;

namespace BMJ.Authenticator.Application.UseCases.Users.Commands.UpdateUser;

public class UpdateUserCommandRequestHandler
    : IRequestHandler<UpdateUserCommandRequest, ResultDto>
{
    private readonly IIdentityAdapter _identityAdapter;
    private readonly IMapper _mapper;

    public UpdateUserCommandRequestHandler(IIdentityAdapter identityService, IMapper mapper)
    {
        _identityAdapter = identityService;
        _mapper = mapper;
    }

    public async Task<ResultDto> Handle(UpdateUserCommandRequest request, CancellationToken cancellationToken)
    {
        using Activity? updateUserActivity = Telemetry.Source.StartActivity("UpdateUserHandler", ActivityKind.Internal);
        updateUserActivity.DisplayName = "MediatR - UpdateUserHandler";

        ResultDto userResult = await _identityAdapter.UpdateUserAsync(request.Id!, request.UserName!, request.Email!, request.PhoneNumber);

        updateUserActivity.SetTag("Succeeded", userResult.Success);

        if (userResult.Success)
            updateUserActivity.AddEvent(new ActivityEvent("User was updated"));
        else
            updateUserActivity.SetTag("Error", userResult.Error);

        return userResult;
    }
}
