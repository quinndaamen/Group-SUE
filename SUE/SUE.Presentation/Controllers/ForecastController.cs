using Microsoft.AspNetCore.Mvc;
using SUE.Services;

[ApiController]
[Route("api/[controller]")]
public class ElectricityController : ControllerBase
{
    private readonly ElectricityPriceForecast _forecast;

    public ElectricityController(ElectricityPriceForecast forecast)
    {
        _forecast = forecast;
    }

    [HttpGet("forecast")]
    public async Task<IActionResult> GetForecast()
    {
        var forecastData = await _forecast.GetForecastAsync();
        return Ok(forecastData);
    }
}