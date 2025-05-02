using System.Text.Json.Serialization;
using Commands;
using Data;
using Handlers;
using Kitchen_Recipe;
using Mappings;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Queries;

try
{
    var builder = WebApplication.CreateBuilder(args);
    
    var firebaseAuthority = builder.Configuration["FirebaseAuth:Authority"]
                            ?? throw new InvalidOperationException("Missing FirebaseAuth:Authority");
    var firebaseAudience = builder.Configuration["FirebaseAuth:Audience"]
                           ?? throw new InvalidOperationException("Missing FirebaseAuth:Audience");
    Console.WriteLine("Authority from config: " + firebaseAuthority);
    Console.WriteLine("Audience from config: " + firebaseAudience);
    builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
        .AddJwtBearer(options =>
        {
            options.Authority = firebaseAuthority;
            options.Audience = firebaseAudience;
            options.RequireHttpsMetadata = true;
        });
    
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
    app.MapEndpoints();
    app.UseCors();
    
    app.Run();
}
catch (Exception ex)
{
    File.WriteAllText("/home/Log.txt", ex.ToString());
    throw;
}

