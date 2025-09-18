using MediatR;
using Sistemaws.Domain.DTOs;

namespace Sistemaws.Application.Queries;

public class GetUserByIdQuery : IRequest<UserResponse?>
{
    public int Id { get; set; }
}
