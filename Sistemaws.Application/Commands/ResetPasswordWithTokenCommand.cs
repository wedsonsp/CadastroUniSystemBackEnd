using MediatR;

namespace Sistemaws.Application.Commands;

public class ResetPasswordWithTokenCommand : IRequest<object>
{
    public string Token { get; set; } = string.Empty;
    public string NewPassword { get; set; } = string.Empty;
}
