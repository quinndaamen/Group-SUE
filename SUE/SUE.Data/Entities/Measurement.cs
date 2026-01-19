using System;

namespace SUE.Data.Entities;

public class Measurement
{
    public Guid Id { get; set; }
    public Guid SensorNodeId { get; set; }
    public SensorNode SensorNode { get; set; } = null!;
    
    public string? P1ID { get; set; }

    public DateTime Timestamp { get; set; }
    // AIR DHT11 + MQ135
    public double? Temperature { get; set; }
    public double? Humidity { get; set; }
    public double? AQI_MQ135 { get; set; }
    
    // P1
    public double? EnergyUsage { get; set; }
    public double? CurrentPowerKW { get; set; }
}