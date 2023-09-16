using BMJ.Authenticator.Adapter;
using BMJ.Authenticator.Api;
using BMJ.Authenticator.Application;
using BMJ.Authenticator.Host;
using BMJ.Authenticator.Infrastructure;
using BMJ.Authenticator.Infrastructure.Persistence;
using HealthChecks.UI.Client;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services
    .AddApplicationServices()
    .AddAdapterServices()
    .AddInfrastructureServices(builder.Configuration)
    .AddApiServices(builder.Configuration)
    .AddHostServices(builder);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseOpenApi();
    app.UseSwaggerUi3();

    if (app.Configuration.GetValue<bool>("InitialiseDatabase"))
    {
        // Initialise and seed database
        using (var scope = app.Services.CreateScope())
        {
            var initialiser = scope.ServiceProvider.GetRequiredService<ApplicationDbContextInitialiser>();
            await initialiser.InitialiseAsync();
            await initialiser.SeedAsync();
        }
    }
}

app.MapHealthChecksUI();
app.MapHealthChecks("/healthz", new Microsoft.AspNetCore.Diagnostics.HealthChecks.HealthCheckOptions()
{
    ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
});
app.UseApiConfiguration();

app.Run();

public partial class Program { }
