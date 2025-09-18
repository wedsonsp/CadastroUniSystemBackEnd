using MediatR;
using Sistemaws.Application.Commands;
using Sistemaws.Domain.DTOs;
using Sistemaws.Domain.Interfaces.Services;
using Sistemaws.Domain.Interfaces.Repositories;
using Sistemaws.Domain.Exceptions;

namespace Sistemaws.Application.Handlers;

public class LoginWithTokenCommandHandler : IRequestHandler<LoginWithTokenCommand, UserResponse>
{
    private readonly IUserRepository _userRepository;
    private readonly IJwtService _jwtService;

    public LoginWithTokenCommandHandler(IUserRepository userRepository, IJwtService jwtService)
    {
        _userRepository = userRepository;
        _jwtService = jwtService;
    }

    public async Task<UserResponse> Handle(LoginWithTokenCommand request, CancellationToken cancellationToken)
    {
        var tokenValidation = _jwtService.ValidateToken(request.Token);
        
        if (!tokenValidation.IsValid)
        {
            throw new DomainException("Invalid token");
        }

        var user = await _userRepository.GetByIdAsync(tokenValidation.UserId);
        
        if (user == null || !user.IsActive)
        {
            throw new DomainException("User not found or inactive");
        }

        return new UserResponse
        {
            Id = user.Id,
            Name = user.Name,
            Email = user.Email,
            IsActive = user.IsActive,
            IsAdministrator = user.IsAdministrator
        };
    }
}




