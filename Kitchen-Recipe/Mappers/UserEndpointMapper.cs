using Commands;
using DataModels;
using Kitchen_Recipe.RequestDTOs;
using Microsoft.EntityFrameworkCore;
using Queries;

namespace Kitchen_Recipe;

public class UserEndpointMapper : IEndpointMapper
{
    
    public void MapEndpoints(IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/").RequireAuthorization();

        group.MapGet("get-user", async (
            [AsParameters] UserRequest req
        ) =>
        {
            var authenticationUid = req.HttpContextAccessor.HttpContext?.GetUserId();
            if (string.IsNullOrWhiteSpace(authenticationUid))
                return Results.Unauthorized();

            var query = new GetUserByAuthUidQuery(authenticationUid);
            var result = await req.Mediator.Send(query);

            return result == null ? Results.NotFound() : Results.Ok(result);
        });

        group.MapPost("add-user", async (
            [AsParameters] AddUserRequest req
        ) =>
        {
            var authenticationUid = req.HttpContextAccessor.HttpContext?.GetUserId();
            if (string.IsNullOrWhiteSpace(authenticationUid))
                return Results.Unauthorized();

            var command = new AddUserCommand(req.userDto, authenticationUid);
            var result = await req.Mediator.Send(command);

            return Results.Ok(result);
        });

        group.MapPut("update-user", async (
            [AsParameters] AddUserRequest req
        ) =>
        {
            var authenticationUid = req.HttpContextAccessor.HttpContext?.GetUserId();
            if (string.IsNullOrWhiteSpace(authenticationUid))
                return Results.Unauthorized();

            var command = new UpdateUserCommand(req.userDto, authenticationUid);
            var result = await req.Mediator.Send(command);

            return result == null ? Results.NotFound() : Results.Ok(result);
        });
        
        group.MapPut("update-user-email", async (
            [AsParameters] AddSocialIdUserRequest req
        ) =>
        {
            var authenticationUid = req.HttpContextAccessor.HttpContext?.GetUserId();
            if (string.IsNullOrWhiteSpace(authenticationUid))
                return Results.Unauthorized();

            var command = new UpdateSocialUserEmailIdCommand(req.UserEmailDto, authenticationUid);
            var result = await req.Mediator.Send(command);

            return result == null ? Results.NotFound() : Results.Ok(result);
        });
    }
}