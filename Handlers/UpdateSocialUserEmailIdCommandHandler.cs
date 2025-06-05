using AutoMapper;
using Commands;
using Data;
using DataModels;
using DTOs;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Handlers;

public class UpdateSocialUserEmailIdCommandHandler : IRequestHandler<UpdateSocialUserEmailIdCommand, UserEmailDto?>
{
    private readonly DataContext _context;
    private readonly IMapper _mapper;

    public UpdateSocialUserEmailIdCommandHandler(DataContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<UserEmailDto?> Handle(UpdateSocialUserEmailIdCommand request, CancellationToken cancellationToken)
    {
        var existingUser =  await _context.UserDetails
            .FirstOrDefaultAsync(u => u.AuthenticationUid == request.AuthenticationUid, cancellationToken);

        if (existingUser == null)
        {
            var newUser = new UserDetails
            {
                AuthenticationUid = request.AuthenticationUid
            };

            _context.UserDetails.Add(newUser);
            await _context.SaveChangesAsync(cancellationToken);

            return _mapper.Map<UserEmailDto>(newUser);
        }
        
        existingUser.EmailId = request.User.EmailId;

        await _context.SaveChangesAsync(cancellationToken);
        return _mapper.Map<UserEmailDto>(existingUser);
    }
}