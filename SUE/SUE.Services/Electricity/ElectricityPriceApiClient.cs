using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

public class ElectricityPriceApiClient
{
    private readonly HttpClient _client;
    private const string Url = "http://127.0.0.1:8000/predict";

    public ElectricityPriceApiClient(HttpClient client)
    {
        _client = client;
    }

    // Expensive operation: NETWORK CALL
    public async Task<List<CleanForecastItem>> FetchForecastAsync()
    {
        var raw = await _client.GetFromJsonAsync<List<ForecastItem>>(Url)
                  ?? throw new Exception("API returned null");

        return raw
            .OrderBy(f => f.Datetime_Local)
            .Select(f => new CleanForecastItem
            {
                Time = f.Datetime_Local, // KEEP DATE, don't nuke it
                PriceEURPerKWh = Math.Round(f.Predicted_Price / 1000, 4)
            })
            .ToList();
    }
}

public class ForecastItem
{
    public DateTime Datetime_Local { get; set; }
    public double Predicted_Price { get; set; } // EUR/MWh
}

public class CleanForecastItem
{
    public DateTime Time { get; set; }
    public double PriceEURPerKWh { get; set; } // EUR/kWh
}