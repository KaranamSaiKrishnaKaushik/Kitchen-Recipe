using DTOs;
using MediatR;

namespace Queries;

public record GetUserByAuthUidQuery(string AuthenticationUid) : IRequest<UserDto?>;