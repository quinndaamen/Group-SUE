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
    // WRITE
    // =========================
    public async Task SaveMeasurementAsync(Guid sensorNodeId, MeasurementDto dto)
    {
        var entity = new Measurement
        {
            Id = Guid.NewGuid(),
            Timestamp = DateTime.UtcNow,

            Temperature = dto.Temperature,
            Humidity = dto.Humidity,
            AQI_MQ135 = dto.AQI,

            EnergyUsage = dto.EnergyTotalKWh,
            CurrentPowerKW = dto.CurrentPowerKW,
        };

        _context.Measurements.Add(entity);
        await _context.SaveChangesAsync();
    }

    // =========================
    // READ
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
        var today = DateTime.UtcNow.Date;

        var first = await _context.Measurements
            .Where(m => m.Timestamp >= today)
            .OrderBy(m => m.Timestamp)
            .Select(m => m.EnergyUsage)
            .FirstOrDefaultAsync();

        var last = await _context.Measurements
            .Where(m => m.Timestamp >= today)
            .OrderByDescending(m => m.Timestamp)
            .Select(m => m.EnergyUsage)
            .FirstOrDefaultAsync();

        return first.HasValue && last.HasValue
            ? last.Value - first.Value
            : 0;
    }

    public async Task<double?> GetMonthEnergyUsageAsync()
    {
        var now = DateTime.UtcNow;
        var monthStart = new DateTime(now.Year, now.Month, 1, 0, 0, 0, DateTimeKind.Utc);

        var first = await _context.Measurements
            .Where(m => m.Timestamp >= monthStart)
            .OrderBy(m => m.Timestamp)
            .Select(m => m.EnergyUsage)
            .FirstOrDefaultAsync();

        var last = await _context.Measurements
            .Where(m => m.Timestamp >= monthStart)
            .OrderByDescending(m => m.Timestamp)
            .Select(m => m.EnergyUsage)
            .FirstOrDefaultAsync();

        return first.HasValue && last.HasValue
            ? last.Value - first.Value
            : 0;
    }
}