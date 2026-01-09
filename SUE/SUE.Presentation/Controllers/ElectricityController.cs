using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace SUE.Presentation.Controllers
{
    public class ElectricityController : Controller
    {
        private readonly ElectricityPriceForecast _forecastService;

        public ElectricityController(ElectricityPriceForecast forecastService)
        {
            _forecastService = forecastService;
        }

        // GET: /Electricity/PriceForecast
        public async Task<IActionResult> PriceForecast()
        {
            List<CleanForecastItem> forecast = await _forecastService.GetForecastAsync();
            return View(forecast); // passes forecast to the view
        }
    }
}