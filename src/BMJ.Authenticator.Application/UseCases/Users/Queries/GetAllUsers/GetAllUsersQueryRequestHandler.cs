using AutoMapper;
using BMJ.Authenticator.Application.Common.Abstractions;
using BMJ.Authenticator.Application.Common.Models.Results;
using BMJ.Authenticator.Application.Common.Models.Users;
using BMJ.Authenticator.Domain.Entities.Users;
using BMJ.Authenticator.Domain.ValueObjects;
using MediatR;

namespace BMJ.Authenticator.Application.UseCases.Users.Queries.GetAllUsers;

public class GetAllUsersQueryRequestHandler
    : IRequestHandler<GetAllUsersQueryRequest, ResultDto<List<UserDto>?>>
{
    private readonly IIdentityAdapter _identityAdapter;
    private readonly IMapper _mapper;

    public GetAllUsersQueryRequestHandler(IIdentityAdapter identityAdapter, IMapper mapper)
    {
        _identityAdapter = identityAdapter;
        _mapper = mapper;
    }

    public async Task<ResultDto<List<UserDto>?>> Handle(GetAllUsersQueryRequest request, CancellationToken cancellationToken)
    {
        var resultDto = await _identityAdapter.GetAllUserAsync();

        if (resultDto.Success)
        {
            var userList = new List<User>();
            foreach (var userDto in resultDto.Value!)
            {
                userList.Add(userDto.ToUser());
            }
            resultDto.Value = _mapper.Map<List<UserDto>>(userList);
        }

        return resultDto;
    }
}
