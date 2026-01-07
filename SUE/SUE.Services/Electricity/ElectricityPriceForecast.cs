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

    public async Task<List<ForecastItem>> GetForecastAsync()
    {
        var url = "http://127.0.0.1:8000/predict"; // your FastAPI endpoint
        var result = await _client.GetFromJsonAsync<List<ForecastItem>>(url);
        return result ?? new List<ForecastItem>();
    }
}

public class ForecastItem
{
    public DateTime Datetime_Local { get; set; }
    public double Predicted_Price { get; set; }
}