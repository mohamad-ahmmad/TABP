using API.Middlewares;
using Application;
using Application.Abstractions;
using Application.Behaviors.Validation;
using Application.Users.Commands.Create;
using Domain.Entities;
using Domain.Repositories;
using FluentValidation;
using Infrastructure.Persistence;
using Infrastructure.Persistence.Repositories.Users;
using Infrastructure.Persistence.UnitOfWork;
using Infrastructure.Security;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Host.UseSerilog((context, config) =>
{
    config.ReadFrom.Configuration(context.Configuration);   
});
builder.Services.AddValidatorsFromAssemblyContaining<ApplicationAssemblyReference>();
builder.Services.AddAutoMapper(typeof(ApplicationAssemblyReference).Assembly);
builder.Services.AddScoped(typeof(IPipelineBehavior<,>), typeof(ValidationPipelineBehavior<,>));
builder.Services.AddScoped<IHasher, Sha256Hasher>();
builder.Services.Configure<JwtOptions>(builder.Configuration.GetSection("JwtOptions"));
builder.Services.AddScoped<IJwtProvider, JwtProvider>();
builder.Services.AddMediatR(config =>
{
    config.RegisterServicesFromAssembly(typeof(ApplicationAssemblyReference).Assembly);
    
});
builder.Services.AddDbContext<TABPDbContext>(config =>
{
    config.UseSqlServer(builder.Configuration["ConnectionStrings:SqlServer"])
    .EnableSensitiveDataLogging();
});
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
builder.Services.AddScoped<IUsersRepository, UsersRepository>();

var app = builder.Build();


// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseMiddleware<ExceptionHandlerMiddleware>();

app.UseSerilogRequestLogging();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
