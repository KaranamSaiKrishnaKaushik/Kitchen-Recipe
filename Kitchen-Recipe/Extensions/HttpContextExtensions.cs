namespace Kitchen_Recipe;

public static class HttpContextExtensions
{
    public static string? GetUserId(this HttpContext context) =>
        context.User.FindFirst("sub")?.Value;
}