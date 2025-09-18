using MediatR;

namespace Sistemaws.Application.Commands;

public class ResetPasswordWithResetTokenCommand : IRequest<object>
{
    public string ResetToken { get; set; } = string.Empty;
    public string NewPassword { get; set; } = string.Empty;
}
