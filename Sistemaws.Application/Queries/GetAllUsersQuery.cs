using MediatR;
using Sistemaws.Domain.DTOs;

namespace Sistemaws.Application.Queries;

public class GetAllUsersQuery : IRequest<IEnumerable<UserResponse>>
{
}
