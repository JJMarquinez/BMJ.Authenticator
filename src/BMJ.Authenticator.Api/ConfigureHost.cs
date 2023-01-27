using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;

namespace BMJ.Authenticator.Api
{
    public static class ConfigureHost
    {
        public static IApplicationBuilder UseApiConfiguration(this IApplicationBuilder app)
        {
            app.UseRouting()
                .UseAuthentication()
                .UseAuthorization()
                .UseEndpoints(endpoints =>
                {
                    endpoints.MapControllers();
                    //endpoints.MapGet("/", async context =>
                    //{
                    //    await context.Response.WriteAsync($"BMJ.Authenticator is running! from {System.Environment.MachineName}");
                    //});
                });

            return app;
        }
    }
}
