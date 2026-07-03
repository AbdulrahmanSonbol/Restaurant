using Contracts.Validator;
using Domain.Contracts;
using Domain.Entities.Identityodule;
using FluentValidation;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Persistence.IdentityData.DataSeed;
using Persistence.IdentityData.DBContexts;
using ServiceAbstraction;
using Services.AuthenticationService;


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
.AddEntityFrameworkStores<RestaurantIdentityDBContexts>();








#endregion

var app = builder.Build();





// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();
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
