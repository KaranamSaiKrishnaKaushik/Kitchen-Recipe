using System.IdentityModel.Tokens.Jwt;
using System.Text.Json.Serialization;
using Commands;
using Data;
using Handlers;
using Kitchen_Recipe;
using Mappings;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Queries;

try
{
    var builder = WebApplication.CreateBuilder(args);

    var firebaseAudience = builder.Configuration["FirebaseAuth:Audience"]
        ?? throw new InvalidOperationException("Missing FirebaseAuth:Audience");
    var firebaseJwk = builder.Configuration["FirebaseAuth:Jwk"]
                           ?? throw new InvalidOperationException("Missing FirebaseAuth:Jwk");

    Console.WriteLine("Audience from config: " + firebaseAudience);

    builder.Services.AddAuthorization();

    builder.Services.AddEndpointsApiExplorer();
    builder.Services.AddSwaggerGen();

    builder.Services.AddSingleton<PayPalService>();
    builder.Services.AddAutoMapper(typeof(AutoMapperProfiles));
    builder.Services.AddScoped<GetRecipeQueryHandler>();
    builder.Services.AddScoped<AddRecipeCommandHandler>();
    builder.Services.AddScoped<GetIngredientQueryHandler>();
    builder.Services.AddScoped<AddIngredientCommandHandler>();
    builder.Services.AddScoped<UpdateRecipeCommandHandler>();
    builder.Services.AddScoped<AddProductsCommandHandler>();
    builder.Services.AddScoped<GetProductsQueryHandler>();

    builder.Services.AddDbContext<DataContext>(options =>
    {
        options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
    });

    builder.Services.AddCors(options =>
    {
        options.AddDefaultPolicy(policy =>
        {
            policy.AllowAnyOrigin()
                  .AllowAnyMethod()
                  .AllowAnyHeader();
        });
    });

    builder.Services.AddControllers().AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.Preserve;
    });

    var app = builder.Build();

    if (app.Environment.IsDevelopment())
    {
        app.UseSwagger();
        app.UseSwaggerUI();
    }

    app.UseSwagger();
    app.UseSwaggerUI();
    app.UseHttpsRedirection();
    app.UseCors();

    /*
         builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
        .AddJwtBearer(options =>
        {
            options.Authority = firebaseAuthority;
            options.Audience = firebaseAudience;
            options.RequireHttpsMetadata = true;
            
            options.Events = new JwtBearerEvents
            {
                OnAuthenticationFailed = context =>
                {
                    Console.WriteLine("JWT AUTH FAIL: " + context.Exception.Message);
                    return Task.CompletedTask;
                },
                OnTokenValidated = context =>
                {
                    Console.WriteLine("JWT AUTH SUCCESS");
                    return Task.CompletedTask;
                }
            };
        });
     */
    
    // Manual JWT Middleware
    app.Use(async (context, next) =>
    {
        var authHeader = context.Request.Headers["Authorization"].FirstOrDefault();

        if (!string.IsNullOrWhiteSpace(authHeader) && authHeader.StartsWith("Bearer "))
        {
            var token = authHeader.Substring("Bearer ".Length);

            try
            {
                var validProjectId = firebaseAudience;
                var http = new HttpClient();
                var jwksJson = await http.GetStringAsync(firebaseJwk);
                Console.WriteLine("JWKS JSON: " + jwksJson);

                var keysResponse = new JsonWebKeySet(jwksJson);

                var tokenHandler = new JwtSecurityTokenHandler();
                var validationParameters = new TokenValidationParameters
                {
                    ValidIssuer = $"https://securetoken.google.com/{validProjectId}",
                    ValidAudience = validProjectId,
                    IssuerSigningKeys = keysResponse.Keys,
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true
                };

                var principal = tokenHandler.ValidateToken(token, validationParameters, out _);
                context.User = principal;

                Console.WriteLine("JWT AUTH SUCCESS for: " + context.User.Identity?.Name);
            }
            catch (Exception ex)
            {
                Console.WriteLine("JWT AUTH FAILED: " + ex.Message);
                context.Response.StatusCode = 401;
                await context.Response.WriteAsync("Unauthorized");
                return;
            }
        }

        await next();
    });

    app.UseAuthorization();
    app.MapEndpoints();

    app.Run();
}
catch (Exception ex)
{
    File.WriteAllText("/home/Log.txt", ex.ToString());
    throw;
}
