using MediatR;
using Sistemaws.Domain.DTOs;

namespace Sistemaws.Application.Commands;

public class LoginWithTokenCommand : IRequest<UserResponse>
{
    public string Token { get; set; } = string.Empty;
}


