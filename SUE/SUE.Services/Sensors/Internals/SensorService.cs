using Microsoft.EntityFrameworkCore;
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

    // =========================
    // WRITE (MQTT → DB)
    // =========================
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

    // =========================
    // READ (Dashboard)
    // =========================
    public async Task<Measurement?> GetLatestMeasurementAsync()
    {
        return await _context.Measurements
            .AsNoTracking()
            .OrderByDescending(m => m.Timestamp)
            .FirstOrDefaultAsync();
    }

    public async Task<double?> GetTodayEnergyUsageAsync()
    {
        var today = DateTime.SpecifyKind(DateTime.UtcNow.Date, DateTimeKind.Utc);;

        return await _context.Measurements
            .AsNoTracking()
            .Where(m => m.Timestamp >= today)
            .Select(m => m.EnergyUsage)
            .MaxAsync();
    }

    public async Task<double?> GetMonthEnergyUsageAsync()
    {
        var now = DateTime.UtcNow;
        var monthStart = new DateTime(now.Year, now.Month, 1, 0, 0, 0, DateTimeKind.Utc);

        return await _context.Measurements
            .AsNoTracking()
            .Where(m => m.Timestamp >= monthStart)
            .Select(m => m.EnergyUsage)
            .MaxAsync();
    }
}
