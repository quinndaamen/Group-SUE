using Microsoft.AspNetCore.Mvc.Razor;

namespace SUE.Presentation.Models;

public class HomeDashboardViewModel
{
    public double? AQTrend;
    public double? CO2;

    // Air
    public double? AQI { get; set; }
    public double? Temperature { get; set; }
    public double? Humidity { get; set; }

    // Electricity
    public double? CurrentPowerKW { get; set; }
    public double? TodayEnergyKWh { get; set; }
    public double? MonthEnergyKWh { get; set; }
    public double? ImprovementPercent { get; set; }
    public double? EstimatedCost { get; set; }
    public int? DevicesOnline { get; set; }
    public int? ActiveRooms { get; set; }
}
