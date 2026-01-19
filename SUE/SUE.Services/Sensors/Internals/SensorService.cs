using SUE.Data;
using SUE.Data.Entities;
using SUE.Services.Sensors.Contracts;

namespace SUE.Services.Sensors.Internals;

internal class SensorService : ISensorService
{
    private readonly AppDbContext _context;

    public SensorService(AppDbContext context)
    {
        _context = context;
    }

    public async Task SaveMeasurementAsync(Guid sensorNodeId, MeasurementDto dto)
    {
        var entity = new Measurement
        {
            Id = Guid.NewGuid(),
            SensorNodeId = sensorNodeId,
            Timestamp = dto.Timestamp,

            Temperature = dto.Temperature,
            Humidity = dto.Humidity,
            AQI_MQ135 = dto.AQI,

            EnergyUsage = dto.EnergyTotalKWh,
            CurrentPowerKW = dto.CurrentPowerKW
        };

        _context.Measurements.Add(entity);
        await _context.SaveChangesAsync();
    }
}