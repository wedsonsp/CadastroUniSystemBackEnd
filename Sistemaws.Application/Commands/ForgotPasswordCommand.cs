using MediatR;
using Sistemaws.Domain.DTOs;

namespace Sistemaws.Application.Commands;

public class ForgotPasswordCommand : IRequest<ForgotPasswordResponse>
{
    public string Email { get; set; } = string.Empty;
}
