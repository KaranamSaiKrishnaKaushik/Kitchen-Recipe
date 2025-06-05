using DTOs;
using MediatR;

namespace Commands;

public record UpdateSocialUserEmailIdCommand(UserEmailDto User, string AuthenticationUid) : IRequest<UserEmailDto>;