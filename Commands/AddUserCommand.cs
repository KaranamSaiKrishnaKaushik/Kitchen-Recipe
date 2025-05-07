using DTOs;
using MediatR;
namespace Commands;

public record AddUserCommand(UserDto UserDto, string AuthenticationUid) : IRequest<UserDto>;