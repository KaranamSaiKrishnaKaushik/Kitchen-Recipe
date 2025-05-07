using DTOs;
using MediatR;

namespace Commands;

public record UpdateUserCommand(UserDto User, string AuthenticationUid) : IRequest<UserDto>;