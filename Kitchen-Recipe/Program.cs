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

var builder = WebApplication.CreateBuilder(args);

//var azureAdOptions = builder.Configuration.GetSection("AzureAd").Get<AzureAdOptions>();

/*builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.Authority = $"{azureAdOptions.Instance}{azureAdOptions.TenantId}/v2.0";
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidIssuer = $"https://sts.windows.net/{azureAdOptions.TenantId}/",
            ValidAudience = $"api://{azureAdOptions.ClientId}"
        }; // [Authorize] each endpoint if you want to use entra ID authorization like //app.MapGet("/", [Authorize]()

    });

builder.Services.AddAuthorization();*/

//builder.Services.AddOpenApi();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(); 

builder.Services.AddSingleton<PayPalService>();
builder.Services.AddAutoMapper(typeof(AutoMapperProfiles));
builder.Services.AddScoped<GetRecipeQueryHandler>();
builder.Services.AddScoped<AddRecipeCommandHandler>();
builder.Services.AddScoped<GetIngredientQueryHandler>();
builder.Services.AddScoped<AddIngredientCommandHandler>();

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

/*if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}*/

app.UseSwagger();
app.UseSwaggerUI();

app.UseHttpsRedirection();
app.MapEndpoints();
app.UseCors();

app.Run();
