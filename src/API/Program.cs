using API.Middlewares;
using Application;
using Application.Abstractions;
using Application.Behaviors.Caching;
using Application.Behaviors.Validation;
using Domain.Repositories;
using FluentValidation;
using Infrastructure.Caching;
using Infrastructure.Persistence;
using Infrastructure.Persistence.Interceptors;
using Infrastructure.Persistence.Repositories.Amenities;
using Infrastructure.Persistence.Repositories.CartItems;
using Infrastructure.Persistence.Repositories.Cities;
using Infrastructure.Persistence.Repositories.Discounts;
using Infrastructure.Persistence.Repositories.Hotels;
using Infrastructure.Persistence.Repositories.HotelTypes;
using Infrastructure.Persistence.Repositories.Owners;
using Infrastructure.Persistence.Repositories.RoomInfos;
using Infrastructure.Persistence.Repositories.Rooms;
using Infrastructure.Persistence.Repositories.RoomTypes;
using Infrastructure.Persistence.Repositories.Users;
using Infrastructure.Persistence.UnitOfWork;
using Infrastructure.Security;
using Infrastructure.Services.Email;
using Infrastructure.Services.ImagesUploaders;
using Infrastructure.Services.Payments;
using Infrastructure.Services.TimeProviders;
using Infrastructure.Services.Uploaders;
using MediatR;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using Serilog;
using Square;
using Square.Apis;
using System.Data.SqlTypes;
using System.Reflection;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers().AddNewtonsoftJson();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{

    options.AddSecurityDefinition("JWTToken", new()
    {
        Type = SecuritySchemeType.Http,
        Scheme = "Bearer",
        Description = "Input a valid token to access this API"
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
builder.Services.AddAutoMapper(typeof(ApplicationAssemblyReference).Assembly, typeof(Program).Assembly);
builder.Services.AddScoped(typeof(IPipelineBehavior<,>), typeof(ValidationPipelineBehavior<,>));
builder.Services.AddScoped(typeof(IPipelineBehavior<,>), typeof(QueryCachingPipelineBehavior<,>));
builder.Services.AddScoped<IHasher, Sha256Hasher>();
builder.Services.Configure<JwtOptions>(builder.Configuration.GetSection(nameof(JwtOptions)));
builder.Services.Configure<EmailConfig>(builder.Configuration.GetSection(nameof(EmailConfig)));
builder.Services.AddSingleton<IImageExtensionValidator, ImageExtensionValidator>();
builder.Services.AddSingleton<IDateTimeProvider, DateTimeProvider>();
builder.Services.AddMemoryCache();
builder.Services.AddSingleton<ICacheService, InMemoryCacheService>();
builder.Services.AddSingleton<IPaymentsApi>(sp =>
{
    var accessToken = builder.Configuration["Square:AccessToken"];
    var squareClient = new SquareClient.Builder()
    .AccessToken(accessToken)
    .Environment(builder.Environment.IsProduction() ? Square.Environment.Production : Square.Environment.Sandbox)
    .Build();
    
    return squareClient.PaymentsApi;
});
builder.Services.Configure<FormOptions>(options =>
{
    options.MultipartBodyLengthLimit = 12000000; //12MB
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
builder.Services.AddDbContext<TABPDbContext>((sp, config) =>
{
    config.UseSqlServer(builder.Configuration["ConnectionStrings:SqlServer"])
                .LogTo(Console.WriteLine,
                    new[] { DbLoggerCategory.Database.Command.Name },
                    LogLevel.Information)
            .EnableSensitiveDataLogging()
            .AddInterceptors(sp.GetRequiredService<AuditableEntityInterceptor>());
});
builder.Services.AddScoped<AuditableEntityInterceptor>();
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
builder.Services.AddScoped<IOwnersRepository, OwnersRepository>();
builder.Services.AddScoped<IHotelTypesRepository, HotelTypesRepository>();
builder.Services.AddScoped<IHotelsRepository, HotelsRepository>();
builder.Services.AddScoped<IRoomTypesRepository, RoomTypesRepository>();
builder.Services.AddScoped<IRoomInfosRepository, RoomInfosRepository>();
builder.Services.AddScoped<IRoomsRepository, RoomsRepository>();
builder.Services.AddScoped<IAmenitiesRepository, AmenitiesRepository>();
builder.Services.AddScoped<IDiscountsRepository, DiscountsRepository>();
builder.Services.AddScoped<ICartItemsRepository, CartItemsRepository>();
builder.Services.AddScoped<IEmailService, EmailService>();
builder.Services.AddScoped<IPaymentService, SquarePaymentService>();
builder.Services.AddSingleton((s) =>
{
    return new JsonSerializerSettings
    {
        ContractResolver = new DefaultContractResolver
        {
            NamingStrategy = new CamelCaseNamingStrategy
            {
                OverrideSpecifiedNames = false,
                ProcessDictionaryKeys = true,
                ProcessExtensionDataNames = true
            }
        }
    };
});
builder.Services.AddHttpContextAccessor();


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
