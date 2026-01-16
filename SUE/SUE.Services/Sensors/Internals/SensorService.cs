using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using SUE.Data;
using SUE.Data;
using SUE.Data.Entities;
using SUE.Services.Sensors.Contracts;
using SUE.Services.Sensors.Models;

public class SensorService : ISensorService
{
    private readonly AppDbContext _context;

    public SensorService(AppDbContext context)
    {
        _context = context;
    }

    public Task<SensorNodeDto> CreateSensorNodeAsync(Guid householdId, string name, string location)
    {
        throw new NotImplementedException();
    }

    public Task<IEnumerable<SensorNodeDto>> GetSensorNodesAsync(Guid householdId)
    {
        throw new NotImplementedException();
    }

    public async Task<MeasurementDto> AddMeasurementAsync(Guid sensorNodeId, MeasurementDto dto)
    {
        var entity = new Measurement
        {
            SensorNodeId = sensorNodeId,
            Timestamp = DateTime.UtcNow,

            // AIR
            Temperature = dto.Temperature,
            Humidity = dto.Humidity,
            AQI_MQ135 = dto.AQI,

            // P1
            EnergyUsage = dto.EnergyTotalKWh
            // CurrentPowerKW is NOT stored (entity does not have it)
        };

        _context.Measurements.Add(entity);
        await _context.SaveChangesAsync();

        return dto;
    }


    public Task<MeasurementDto> GetMeasurementsAsync()
    {
        throw new NotImplementedException();
    }
}
