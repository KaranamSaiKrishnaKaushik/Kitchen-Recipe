namespace Kitchen_Recipe;

public static class WebApplicationExtensions
{
    public static WebApplication ConfigureMiddlewares(this WebApplication app)
    {
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.UseHttpsRedirection();
        app.UseCors();
        app.UseAuthentication();
        app.UseAuthorization();

        return app;
    }

    public static WebApplication RegisterEndpoints(this WebApplication app)
    {
        using var scope = app.Services.CreateScope();
        var mapperFactory = scope.ServiceProvider.GetRequiredService<IEndpointMapperFactory>();
        foreach (var mapper in mapperFactory.GetEndpointMappers())
        {
            mapper.MapEndpoints(app);
        }

        return app;
    }
}