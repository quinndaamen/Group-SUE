using SUE.Services.Sensors.Models;

namespace SUE.Services.Sensors.Contracts;

public interface IGraphService
{
    Task<List<HourlyValueDto>> GetAqiLast4HoursAsync();
    Task<List<HourlyValueDto>> GetElectricityLast4HoursAsync();
}