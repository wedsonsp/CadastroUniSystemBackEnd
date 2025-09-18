using MediatR;
using Sistemaws.Application.Queries;
using Sistemaws.Domain.DTOs;
using Sistemaws.Domain.Interfaces.Repositories;

namespace Sistemaws.Application.Handlers;

public class GetAllUsersQueryHandler : IRequestHandler<GetAllUsersQuery, IEnumerable<UserResponse>>
{
    private readonly IUserRepository _userRepository;

    public GetAllUsersQueryHandler(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    public async Task<IEnumerable<UserResponse>> Handle(GetAllUsersQuery request, CancellationToken cancellationToken)
    {
        var users = await _userRepository.GetAllAsync();

        return users.Select(user => new UserResponse
        {
            Id = user.Id,
            Name = user.Name,
            Email = user.Email,
            CreatedAt = user.CreatedAt ?? DateTime.UtcNow,
            UpdatedAt = user.UpdatedAt,
            IsActive = user.IsActive
        });
    }
}
