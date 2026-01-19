using SUE.Services.Sensors.Models;

namespace SUE.Presentation.Models;

public class GraphsViewModel
{
    public List<HourlyValueDto> AqiLast4Hours { get; set; } = [];
    public List<HourlyValueDto> ElectricityLast4Hours { get; set; } = [];
}