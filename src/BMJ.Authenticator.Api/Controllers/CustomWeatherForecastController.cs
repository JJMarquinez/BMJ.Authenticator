using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Runtime.InteropServices;
using System.Xml.Linq;

namespace BMJ.Authenticator.Api.Controllers;

[ApiController]
[Route("[controller]")]
public class CustomWeatherForecastController : ControllerBase
{
    private static readonly string[] Summaries = new[]
    {
    "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
};

    private readonly ILogger<CustomWeatherForecastController> _logger;

    public CustomWeatherForecastController(ILogger<CustomWeatherForecastController> logger)
    {
        _logger = logger;
    }

    [Authorize]
    [HttpGet(Name = "CustomGetWeatherForecast")]
    public IEnumerable<CustomWeatherForecast> Get()
    {
        return Enumerable.Range(1, 5).Select(index => new CustomWeatherForecast
        {
            Date = DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
            TemperatureC = Random.Shared.Next(-20, 55),
            Summary = Summaries[Random.Shared.Next(Summaries.Length)]
        })
        .ToArray();
    }
}
