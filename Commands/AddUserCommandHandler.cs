using AutoMapper;
using DTOs;

namespace Commands;

using AutoMapper;
using Data;
using MediatR;
using DataModels;

public class AddUserCommandHandler : IRequestHandler<AddUserCommand, UserDto>
{
    private readonly DataContext _context;
    private readonly IMapper _mapper;

    public AddUserCommandHandler(DataContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<UserDto> Handle(AddUserCommand request, CancellationToken cancellationToken)
    {
        var userEntity = _mapper.Map<UserDetails>(request.UserDto);
        userEntity.AuthenticationUid = request.AuthenticationUid;
        _context.UserDetails.Add(userEntity);
        await _context.SaveChangesAsync(cancellationToken);
        return _mapper.Map<UserDto>(userEntity);
    }
}
