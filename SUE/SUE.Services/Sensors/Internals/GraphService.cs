using Microsoft.EntityFrameworkCore;
using SUE.Data;
using SUE.Services.Sensors.Contracts;
using SUE.Services.Sensors.Models;

namespace SUE.Services.Sensors.Internals;

internal class GraphService : IGraphService
{
    private readonly AppDbContext _context;

    public GraphService(AppDbContext context)
    {
        _context = context;
    }

    public async Task<List<HourlyValueDto>> GetAqiLast4HoursAsync()
    {
        var since = DateTime.UtcNow.AddHours(-4);

        return await _context.Measurements
            .AsNoTracking()
            .Where(m => m.Timestamp >= since && m.AQI_MQ135 != null)
            .GroupBy(m => new
            {
                m.Timestamp.Year,
                m.Timestamp.Month,
                m.Timestamp.Day,
                m.Timestamp.Hour
            })
            .Select(g => new HourlyValueDto
            {
                Hour = g.Key.Hour,
                Value = g.Average(x => x.AQI_MQ135)
            })
            .OrderBy(x => x.Hour)
            .ToListAsync();
    }

    public async Task<List<HourlyValueDto>> GetElectricityLast4HoursAsync()
    {
        var since = DateTime.UtcNow.AddHours(-4);

        return await _context.Measurements
            .AsNoTracking()
            .Where(m => m.Timestamp >= since && m.CurrentPowerKW != null)
            .GroupBy(m => new
            {
                m.Timestamp.Year,
                m.Timestamp.Month,
                m.Timestamp.Day,
                m.Timestamp.Hour
            })
            .Select(g => new HourlyValueDto
            {
                Hour = g.Key.Hour,
                Value = g.Average(x => x.CurrentPowerKW)
            })
            .OrderBy(x => x.Hour)
            .ToListAsync();
    }
}
