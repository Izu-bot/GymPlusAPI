using System.Security.Claims;
using System.Text;
using DotNetEnv;
using FluentValidation;
using FluentValidation.AspNetCore;
using GymPlusAPI.API.Filters;
using GymPlusAPI.Application.Auth;
using GymPlusAPI.Application.Interfaces;
using GymPlusAPI.Application.Services;
using GymPlusAPI.Application.Validator;
using GymPlusAPI.Domain.Interfaces;
using GymPlusAPI.Infrastructure.Data;
using GymPlusAPI.Infrastructure.Persistence;
using GymPlusAPI.Infrastructure.Security;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.OpenApi;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Scalar.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddCors(options => {
    options.AddPolicy("AllowAllOrigins",
        policy =>
        {
            policy.AllowAnyOrigin()
                .AllowAnyMethod()
                .AllowAnyHeader();
        });
});

#region load variables .env
Env.Load();
builder.Configuration.AddEnvironmentVariables();
#endregion

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi("v1", options => { options.AddDocumentTransformer<BearerSecuritySchemeTransformer>(); });

// var connectionString = _configuration.GetConnectionString("DefaultConnection");
const string connectionString = $"User ID=postgres;Password=3510;Host=localhost;Port=5432;Database=GymPlus;Pooling=true;";

builder.Services.AddDbContext<AppDbContext>(options =>
{
    options.UseNpgsql(connectionString);
});

builder.Services.AddFluentValidationAutoValidation();
builder.Services.AddValidatorsFromAssemblyContaining<ValidationAsemblyMarker>();

builder.Services.AddControllers(options =>
{
    options.Filters.Add<CustomExceptionFilter>();
});
builder.Services.AddScoped<CustomExceptionFilter>();

builder.Services.AddScoped<ISpreadsheetRepository, SpreadsheetRepository>();
builder.Services.AddScoped<IWorkoutRepository, WorkoutRepository>();
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<ICustomMuscleGroupRepository, CustomMuscleGroupRepository>();
builder.Services.AddScoped<IRecurrentTrainingRepository, RecurrentTrainingRepository>();
builder.Services.AddScoped<ISpreadsheetService, SpreadsheetService>();
builder.Services.AddScoped<IWorkoutService, WorkoutService>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<ICustomMuscleGroupService, CustomMuscleGroupService>();
builder.Services.AddScoped<IRecurrentTrainingService, RecurrentTrainingService>();
builder.Services.AddScoped<IPasswordHasher, PasswordHasher>();
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<IJwtGenerator, JwtGenerator>();

#region Configuração JWT

builder.Services.AddAuthentication(options =>
    {
        options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
        options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    })
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            // Valida quem está gerando o token
            ValidateIssuer = true,
            ValidIssuer = builder.Configuration["JwtSettings:Issuer"],

            // Valida para quem o token foi gerado
            ValidateAudience = true,
            ValidAudience = builder.Configuration["JwtSettings:Audience"],

            // Valida se o token ainda não expirou
            ValidateLifetime = true,

            // Valida a chave de assinatura para garantir que o token não foi alterado
            ValidateIssuerSigningKey = true,
            IssuerSigningKey =
                new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["JwtSettings:Key"]!)),

            // Permite uma pequena margem de tempo caso os relógios do servidor e cliente não estejam perfeitamente sincronizados
            ClockSkew = TimeSpan.Zero
        };
    });
#endregion

// Adicionar serviços de autorização
builder.Services.AddAuthorization();

var app = builder.Build();

app.UseCors("AllowAllOrigins");

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference();

    app.MapGet("/", context =>
    {
        context.Response.Redirect("scalar/v1");
        return Task.CompletedTask;
    });
}
app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();
app.UseHttpsRedirection();
app.Run();

internal sealed class BearerSecuritySchemeTransformer(Microsoft.AspNetCore.Authentication.IAuthenticationSchemeProvider authenticationSchemeProvider) : IOpenApiDocumentTransformer
{
    public async Task TransformAsync(OpenApiDocument document, OpenApiDocumentTransformerContext context, CancellationToken cancellationToken)
    {
        var authenticationSchemes = await authenticationSchemeProvider.GetAllSchemesAsync();
        if (authenticationSchemes.Any(authScheme => authScheme.Name == "Bearer"))
        {
            var requirements = new Dictionary<string, OpenApiSecurityScheme>
            {
                // Teste
                ["Bearer"] = new OpenApiSecurityScheme
                {
                    Type = SecuritySchemeType.Http,
                    Scheme = "bearer", 
                    In = ParameterLocation.Header,
                    BearerFormat = "Json Web Token"
                }
            };
            document.Components ??= new OpenApiComponents();
            document.Components.SecuritySchemes = requirements;

            foreach (var operation in document.Paths.Values.SelectMany(path => path.Operations))
            {
                operation.Value.Security.Add(new OpenApiSecurityRequirement
                {
                    [new OpenApiSecurityScheme { Reference = new OpenApiReference { Id = "Bearer", Type = ReferenceType.SecurityScheme } }] = []
                });
            }
        }
    }
};