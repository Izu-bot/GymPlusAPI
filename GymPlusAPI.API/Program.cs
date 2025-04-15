using GymPlusAPI.Application.Interfaces;
using GymPlusAPI.Application.Services;
using GymPlusAPI.Domain.Interfaces;
using GymPlusAPI.Infrastructure.Data;
using GymPlusAPI.Infrastructure.Persistence;
using GymPlusAPI.Infrastructure.Security;
using Microsoft.EntityFrameworkCore;
using Scalar.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

var connectionString = builder.Configuration.GetConnectionString("databaseConnection");

builder.Services.AddDbContext<AppDbContext>(options =>
{
    options.UseNpgsql(connectionString);
});

builder.Services.AddControllers();

builder.Services.AddScoped<ISpreadsheetRepository, SpreadsheetRepository>();
builder.Services.AddScoped<IWorkoutRepository, WorkoutRepository>();
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<ISpreadsheetService, SpreadsheetService>();
builder.Services.AddScoped<IWorkoutService, WorkoutService>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IPasswordHasher, PasswordHasher>();

var app = builder.Build();

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
app.MapControllers();
app.UseHttpsRedirection();
app.Run();

// Criar IService no Application e implementar no Infrastructure, Reescrever o metodo DeleteAsync no repository
// Implementar o Usuario e classe LoggedUser para buscar o Id pela sessão
// Arrumar o controller para não passar o ID pela URL