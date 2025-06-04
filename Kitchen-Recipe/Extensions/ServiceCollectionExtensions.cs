using System.IdentityModel.Tokens.Jwt;
using System.Text.Json.Serialization;
using Commands;
using Data;
using Handlers;
using Mappings;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Queries;

namespace Kitchen_Recipe;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection RegisterAppConfiguration(this IServiceCollection services, IConfiguration config)
    {
        services.Configure<AzureTranslatorOptions>(config.GetSection("AzureTranslator"));
        return services;
    }

    public static IServiceCollection RegisterDependencies(this IServiceCollection services, IConfiguration config)
    {
        // Auth0
        JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Clear();
        services.AddAuthentication("Bearer")
            .AddJwtBearer("Bearer", options =>
            {
                options.Authority = $"https://{config["Auth0:Domain"]}/";
                options.Audience = config["Auth0:Audience"];
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidIssuer = $"https://{config["Auth0:Domain"]}/",
                    ValidateAudience = true,
                    ValidAudience = config["Auth0:Audience"],
                    ValidateLifetime = true
                };
            });

        services.AddAuthorization();

        // MediatR
        services.AddMediatR(cfg =>
            cfg.RegisterServicesFromAssemblyContaining<AddUserCommand>());
        services.AddMediatR(cfg =>
            cfg.RegisterServicesFromAssembly(typeof(GetUserByAuthUidQueryHandler).Assembly));
        services.AddMediatR(cfg =>
            cfg.RegisterServicesFromAssembly(typeof(UpdateUserCommandHandler).Assembly));

        // Automapper
        services.AddAutoMapper(
            typeof(AutoMapperProfiles).Assembly,
            typeof(UserProfile).Assembly
        );

        // Scoped Handlers
        services.Scan(scan => scan
            .FromAssemblyOf<AddRecipeCommandHandler>()
            .AddClasses(classes => classes.Where(type => type.Name.EndsWith("Handler")))
            .AsSelf()
            .WithScopedLifetime());
        services.AddScoped<GetProductsQueryHandler>();
        services.AddScoped<GetShoppingCartQueryHandler>();

        // Endpoint Mappers
        services.Scan(scan => scan
            .FromAssemblyOf<IEndpointMapper>()
            .AddClasses(classes => classes.AssignableTo<IEndpointMapper>())
            .As<IEndpointMapper>()
            .WithScopedLifetime());

        services.AddScoped<IEndpointMapperFactory, EndpointMapperFactory>();

        // External Services
        services.AddSingleton<PayPalService>();
        services.AddHttpClient();

        // EF Core
        services.AddDbContext<DataContext>(options =>
            options.UseSqlServer(config.GetConnectionString("DefaultConnection")));

        // Swagger
        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen();
        
        services.AddHttpContextAccessor();

        // JSON & CORS
        services.AddControllers().AddJsonOptions(options =>
        {
            options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.Preserve;
        });

        services.AddCors(options =>
        {
            options.AddDefaultPolicy(policy =>
            {
                policy.AllowAnyOrigin()
                      .AllowAnyMethod()
                      .AllowAnyHeader();
            });
        });

        return services;
    }
}
