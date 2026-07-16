using Contracts.Validator;
using Domain.Contracts;
using Domain.Entities.IdentityModule;
using FirebaseAdmin;
using FluentValidation;
using Google.Apis.Auth.OAuth2;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder.Extensions;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Persistence.BackgroundJobs;
using Persistence.IdentityData.DataSeed;
using Persistence.IdentityData.DBContexts;
using Persistence.Repositories;
using Restaurant.CustomMiddlewares;
using ServiceAbstraction;
using Services.AuthenticationService;
using Services.EmailService;
using Services.Mapping;
using Services.Notification;
using Services.Restaurant;
using StackExchange.Redis;
using System.Text;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

#region Add services to the container


builder.Services.AddValidatorsFromAssemblyContaining<RegisterDTOValidator>();

builder.Services.AddScoped<IAuthenticationService, AuthenticationService>();

builder.Services.AddDbContext<RestaurantIdentityDBContexts>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("IdentityConnection"));
});

builder.Services.AddKeyedScoped<IDataInitializer, IdentityDataInitializer>("Identity");

builder.Services.AddIdentity<User, IdentityRole<Guid>>(options =>
{
    options.Password.RequiredLength = 7;
    options.Password.RequireDigit = true;
    options.Password.RequireLowercase = true;
    options.Password.RequireUppercase = true;
    options.Password.RequireNonAlphanumeric = true;
    options.User.RequireUniqueEmail = true;



})
.AddRoles<IdentityRole<Guid>>()
.AddEntityFrameworkStores<RestaurantIdentityDBContexts>()
.AddDefaultTokenProviders();


builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,

        ValidIssuer = builder.Configuration["JWTOptions:Issuer"],
        ValidAudience = builder.Configuration["JWTOptions:Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(
            Encoding.UTF8.GetBytes(builder.Configuration["JWTOptions:SecretKey"]!))
    };
});



builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyMethod()
              .AllowAnyHeader();
    });
});

builder.Services.AddAutoMapper(cfg =>
{
    cfg.AddProfile<UserMapping>();
    cfg.AddProfile<ReservationMapping>();
});


builder.Services.AddTransient<IEmailService, EmailService>();

var redisOptions = ConfigurationOptions.Parse(
    builder.Configuration.GetConnectionString("RedisConnection")!
);

IConnectionMultiplexer redisMultiplexer =
    await ConnectionMultiplexer.ConnectAsync(redisOptions);

builder.Services.AddSingleton<IConnectionMultiplexer>(redisMultiplexer);

builder.Services.AddStackExchangeRedisCache(options =>
{
    options.ConnectionMultiplexerFactory = () =>
        Task.FromResult(redisMultiplexer);
});

builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

builder.Services.AddScoped(typeof(IGenericRepository<,>), typeof(GenericRepository<,>));

//using (var stream = new FileStream("firebase-config.json", FileMode.Open, FileAccess.Read))
//{
//    var googleCredential = CredentialFactory.FromStream<GoogleCredential>(stream);

//    FirebaseApp.Create(new AppOptions()
//    {
//        Credential = googleCredential
//    });
//}

builder.Services.AddScoped<INotificationService, NotificationService>();

builder.Services.AddHostedService<ReservationReminderBackgroundJob>();

builder.Services.AddScoped<IReservationService, ReservationService>();
#endregion

var app = builder.Build();

app.UseMiddleware<GlobalExceptionHandlerMiddleware>();
// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();
app.UseCors("AllowAll");
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

#region Run Identity Data Seeder

using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    try
    {
        var identityInitializer = services.GetKeyedService<IDataInitializer>("Identity");

        if (identityInitializer != null)
        {
            await identityInitializer.InitializeAsync();
        }
    }
    catch (Exception ex)
    {
        var logger = services.GetRequiredService<ILogger<Program>>();
        logger.LogError(ex, "Error Identity Data Seed");
    }
}

#endregion


app.Run();
