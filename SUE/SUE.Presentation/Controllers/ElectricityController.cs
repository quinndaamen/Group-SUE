using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace SUE.Presentation.Controllers
{
    public class ElectricityController : Controller
    {
        private readonly ElectricityPriceCacheService _priceService;

        public ElectricityController(ElectricityPriceCacheService priceService)
        {
            _priceService = priceService;
        }

        // GET: /Electricity/Details
        public async Task<IActionResult> Details()
        {
            List<CleanForecastItem> forecast = await _priceService.GetForecastAsync();

            return View(forecast); // automatically looks for Details.cshtml
        }
    }
}