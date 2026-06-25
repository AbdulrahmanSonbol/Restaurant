using Contracts.Validator;
using FluentValidation;
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

app.Run();
