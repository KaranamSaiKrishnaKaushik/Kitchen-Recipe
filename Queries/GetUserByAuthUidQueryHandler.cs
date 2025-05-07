using AutoMapper;
using DTOs;

namespace Queries;

using Data;
using MediatR;
using Microsoft.EntityFrameworkCore;

public class GetUserByAuthUidQueryHandler : IRequestHandler<GetUserByAuthUidQuery, UserDto?>
{
    private readonly DataContext _context;
    private readonly IMapper _mapper;

    public GetUserByAuthUidQueryHandler(DataContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<UserDto?> Handle(GetUserByAuthUidQuery request, CancellationToken cancellationToken)
    {
        var user = await _context.UserDetails
            .FirstOrDefaultAsync(u=>u.AuthenticationUid == request.AuthenticationUid, cancellationToken);
        
        return user == null ? null : _mapper.Map<UserDto>(user);
    }
}
