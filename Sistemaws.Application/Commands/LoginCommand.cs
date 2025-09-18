using MediatR;
using Sistemaws.Domain.DTOs;

namespace Sistemaws.Application.Commands;

public class LoginCommand : IRequest<LoginResponse>
{
    public string Email { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
}
