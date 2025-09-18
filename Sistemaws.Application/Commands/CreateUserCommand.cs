using MediatR;
using Sistemaws.Domain.DTOs;
using Sistemaws.Domain.Entities;

namespace Sistemaws.Application.Commands;

public class CreateUserCommand : IRequest<UserResponse>
{
    public string Name { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
    public string Token { get; set; } = string.Empty;
}
