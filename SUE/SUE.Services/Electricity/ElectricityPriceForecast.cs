using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

public class ElectricityPriceForecast
{
    private readonly HttpClient _client;

    public ElectricityPriceForecast(HttpClient client)
    {
        _client = client;
    }

    public async Task<List<CleanForecastItem>> GetForecastAsync()
    {
        var url = "http://127.0.0.1:8000/predict"; // FastAPI endpoint
        var raw = await _client.GetFromJsonAsync<List<ForecastItem>>(url) ?? new List<ForecastItem>();

        // Clean & convert MWh → kWh
        return raw
            .OrderBy(f => f.Datetime_Local)
            .Select(f => new CleanForecastItem
            {
                Time = f.Datetime_Local.ToString("HH:mm"),
                PriceEURPerKWh = Math.Round(f.Predicted_Price / 1000, 4)
            })
            .ToList();
    }
}

// Raw item from API
public class ForecastItem
{
    public DateTime Datetime_Local { get; set; }
    public double Predicted_Price { get; set; } // EUR/MWh
}

// Cleaned item for frontend display
public class CleanForecastItem
{
    public string Time { get; set; }
    public double PriceEURPerKWh { get; set; } // EUR/kWh
}