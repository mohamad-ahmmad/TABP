using API.Middlewares;
using Application;
using Application.Abstractions;
using Application.Behaviors.Validation;
using Domain.Repositories;
using FluentValidation;
using Infrastructure.Persistence;
using Infrastructure.Persistence.Repositories.Cities;
using Infrastructure.Persistence.Repositories.Users;
using Infrastructure.Persistence.UnitOfWork;
using Infrastructure.Security;
using Infrastructure.Services.ImagesUploaders;
using Infrastructure.Services.Uploaders;
using MediatR;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Serilog;
using System.Reflection;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{

    options.AddSecurityDefinition("JWTToken", new()
    {
        Type = SecuritySchemeType.Http,
        Scheme="Bearer",
        Description="Input a valid token to access this API"
    });


    var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
    options.IncludeXmlComments(xmlPath);
});
builder.Host.UseSerilog((context, config) =>
{
    config.ReadFrom.Configuration(context.Configuration);
});
builder.Services.AddValidatorsFromAssemblyContaining<ApplicationAssemblyReference>();
builder.Services.AddAutoMapper(typeof(ApplicationAssemblyReference).Assembly);
builder.Services.AddScoped(typeof(IPipelineBehavior<,>), typeof(ValidationPipelineBehavior<,>));
builder.Services.AddScoped<IHasher, Sha256Hasher>();
builder.Services.Configure<JwtOptions>(builder.Configuration.GetSection(nameof(JwtOptions)));
builder.Services.AddSingleton<IImageExtensionValidator, ImageExtensionValidator>();
builder.Services.Configure<FormOptions>(options =>
{
    options.MultipartBodyLengthLimit = 12000000; 
});
builder.Services.AddAuthentication("Bearer")
    .AddJwtBearer(options =>
    {
        var jwtOptions = builder.Configuration.GetSection(nameof(JwtOptions)).Get<JwtOptions>()!;
        var key = Encoding.UTF8.GetBytes(jwtOptions.Key!);

        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = jwtOptions.Issuer,
            ValidAudience = jwtOptions.Audience,
            IssuerSigningKey = new SymmetricSecurityKey(key),
            ClockSkew = TimeSpan.Zero
        };
    });

builder.Services.AddScoped<IJwtProvider, JwtProvider>();
builder.Services.AddHttpContextAccessor();
builder.Services.AddMediatR(config =>
{
    config.RegisterServicesFromAssembly(typeof(ApplicationAssemblyReference).Assembly);

});
builder.Services.AddDbContext<TABPDbContext>(config =>
{
    config.UseSqlServer(builder.Configuration["ConnectionStrings:SqlServer"])
                .LogTo(Console.WriteLine,
                    new[] { DbLoggerCategory.Database.Command.Name },
                    LogLevel.Information)
            .EnableSensitiveDataLogging();
});
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
builder.Services.AddScoped<IUsersRepository, UsersRepository>();
builder.Services.AddScoped<IImageUploaderService>(provider =>
{
    var uploader = new InServerImageUploaderService();
    uploader.SetWebRootPath(builder.Environment.WebRootPath);

    return uploader;
});
builder.Services.AddScoped<IUserContext, UserContext>();
builder.Services.AddScoped<ICitiesRepository, CitiesRepository>();

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

app.UseStaticFiles();

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();
