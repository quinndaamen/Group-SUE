public class MeasurementDto
{
    public DateTime Timestamp { get; set; } = DateTime.UtcNow;

    // DHT11 + MQ135
    public double? Temperature { get; set; }
    public double? Humidity { get; set; }
    public double? AQI { get; set; }

    // P1
    public double? EnergyTotalKWh { get; set; }    // total kWh
    public double? CurrentPowerKW { get; set; }    // current kW
}