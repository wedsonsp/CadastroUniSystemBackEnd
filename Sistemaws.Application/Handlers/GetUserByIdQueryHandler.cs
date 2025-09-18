using MediatR;
using Sistemaws.Application.Queries;
using Sistemaws.Domain.DTOs;
using Sistemaws.Domain.Interfaces.Repositories;

namespace Sistemaws.Application.Handlers;

public class GetUserByIdQueryHandler : IRequestHandler<GetUserByIdQuery, UserResponse?>
{
    private readonly IUserRepository _userRepository;

    public GetUserByIdQueryHandler(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    public async Task<UserResponse?> Handle(GetUserByIdQuery request, CancellationToken cancellationToken)
    {
        var user = await _userRepository.GetByIdAsync(request.Id);

        if (user == null)
            return null;

        return new UserResponse
        {
            Id = user.Id,
            Name = user.Name,
            Email = user.Email,
            CreatedAt = user.CreatedAt ?? DateTime.UtcNow,
            UpdatedAt = user.UpdatedAt,
            IsActive = user.IsActive,
            IsAdministrator = user.IsAdministrator
        };
    }
}
