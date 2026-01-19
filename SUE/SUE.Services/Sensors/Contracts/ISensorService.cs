using SUE.Services.Sensors.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using SUE.Data.Entities;

namespace SUE.Services.Sensors.Contracts
{
    public interface ISensorService
    {
        Task SaveMeasurementAsync(Guid sensorNodeId, MeasurementDto dto);

        Task<Measurement?> GetLatestMeasurementAsync();
        Task<double?> GetTodayEnergyUsageAsync();
        Task<double?> GetMonthEnergyUsageAsync();
    }
}