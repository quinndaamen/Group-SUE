using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using System.Collections.Generic;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;


namespace SUE.Presentation.Controllers
{
    [Authorize(AuthenticationSchemes = CookieAuthenticationDefaults.AuthenticationScheme)]
    public class DetailsController : Controller
    {
        private readonly ElectricityPriceCacheService _priceService;

        public DetailsController(ElectricityPriceCacheService priceService)
        {
            _priceService = priceService;
        }

        public IActionResult Air()
        {
            return View();
        }

        

        // GET: /Electricity/Details
        public async Task<IActionResult> Electricity()
        {
            List<CleanForecastItem> forecast = await _priceService.GetForecastAsync();

            return View(forecast);
        }
    }
}