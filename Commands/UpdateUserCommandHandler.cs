using DataModels;

namespace Commands;

using AutoMapper;
using Data;
using DTOs;
using MediatR;
using Microsoft.EntityFrameworkCore;

public class UpdateUserCommandHandler : IRequestHandler<UpdateUserCommand, UserDto?>
{
    private readonly DataContext _context;
    private readonly IMapper _mapper;

    public UpdateUserCommandHandler(DataContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<UserDto?> Handle(UpdateUserCommand request, CancellationToken cancellationToken)
    {
        var existingUser =  await _context.UserDetails
            .FirstOrDefaultAsync(u => u.AuthenticationUid == request.AuthenticationUid, cancellationToken);

        if (existingUser == null)
        {
            var newUser = new UserDetails
            {
                AuthenticationUid = request.AuthenticationUid,
                UserFirstName = null,
                UserLastName = null,
            };

            _context.UserDetails.Add(newUser);
            await _context.SaveChangesAsync(cancellationToken);

            return _mapper.Map<UserDto>(newUser);
        }
        
        if (!string.IsNullOrWhiteSpace(request.User.UserFirstName))
            existingUser.UserFirstName = request.User.UserFirstName;
        
        _mapper.Map(request.User, existingUser);
        _context.Entry(existingUser).CurrentValues.SetValues(request.User);
        existingUser.AuthenticationUid = request.AuthenticationUid;
        await _context.SaveChangesAsync(cancellationToken);
        return _mapper.Map<UserDto>(existingUser);
    }
}
