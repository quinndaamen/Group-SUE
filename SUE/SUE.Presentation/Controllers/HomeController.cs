using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SUE.Presentation.Models;
using SUE.Services.Sensors.Contracts;

namespace SUE.Presentation.Controllers;

[Authorize(AuthenticationSchemes = CookieAuthenticationDefaults.AuthenticationScheme)]
public class HomeController : Controller
{
    private readonly ISensorService _sensorService;

    public HomeController(ISensorService sensorService)
    {
        _sensorService = sensorService;
    }

    public async Task<IActionResult> Index()
    {
        var latest = await _sensorService.GetLatestMeasurementAsync();

        var model = new HomeDashboardViewModel
        {
            AQI = latest?.AQI_MQ135,
            Temperature = latest?.Temperature,
            Humidity = latest?.Humidity,

            CurrentPowerKW = latest?.CurrentPowerKW,
            TodayEnergyKWh = await _sensorService.GetTodayEnergyUsageAsync(),
            MonthEnergyKWh = await _sensorService.GetMonthEnergyUsageAsync()
        };

        return View(model);
    }
}