using Data;
using DTOs;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Kitchen_Recipe.RequestDTOs;

public class UserRequest
{
    [FromServices]
    public IHttpContextAccessor HttpContextAccessor { get; set; }
    
    [FromServices] 
    public IMediator Mediator { get; set; }
}

public class AddUserRequest
{
    [FromBody]
    public UserDto userDto  { get; set; }
    
    [FromServices]
    public IHttpContextAccessor HttpContextAccessor { get; set; }
    
    [FromServices] 
    public IMediator Mediator { get; set; }
}

public class AddSocialIdUserRequest
{
    [FromBody]
    public UserEmailDto UserEmailDto  { get; set; }
    
    [FromServices]
    public IHttpContextAccessor HttpContextAccessor { get; set; }
    
    [FromServices] 
    public IMediator Mediator { get; set; }
    
    [FromServices]
    public DataContext Context { get; set; }
}