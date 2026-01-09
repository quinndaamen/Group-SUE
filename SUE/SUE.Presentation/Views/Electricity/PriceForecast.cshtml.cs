using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SUE.Presentation.Views.Electricity.PriceForecast
{
    public class PriceForecastModel : PageModel
    {
        private readonly ElectricityPriceForecast _forecastService;

        public PriceForecastModel(ElectricityPriceForecast forecastService)
        {
            _forecastService = forecastService;
        }

        // This will be available in the Razor page
        public List<CleanForecastItem> Forecast { get; set; } = new List<CleanForecastItem>();

        public async Task OnGetAsync()
        {
           
        }
    }
}