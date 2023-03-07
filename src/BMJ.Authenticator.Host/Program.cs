using BMJ.Authenticator.Api;
using BMJ.Authenticator.Application;
using BMJ.Authenticator.Host;
using BMJ.Authenticator.Infrastructure;
using BMJ.Authenticator.Infrastructure.Persistence;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services
    .AddApplicationServices()
    .AddInfrastructureServices(builder.Configuration)
    .AddApiServices(builder.Configuration)
    .AddHostServices(builder);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseOpenApi();
    app.UseSwaggerUi3();

    // Initialise and seed database
    using (var scope = app.Services.CreateScope())
    {
        var initialiser = scope.ServiceProvider.GetRequiredService<ApplicationDbContextInitialiser>();
        await initialiser.InitialiseAsync();
        await initialiser.SeedAsync();
    }
}

//app.UseHttpsRedirection();

app.UseApiConfiguration();

app.Run();
