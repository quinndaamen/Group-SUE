using SUE.Services.Sensors.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SUE.Services.Sensors.Contracts
{
    public interface ISensorService
    {
        Task<SensorNodeDto> CreateSensorNodeAsync(Guid householdId, string name, string location);
        Task<IEnumerable<SensorNodeDto>> GetSensorNodesAsync(Guid householdId);
        Task<MeasurementDto> AddMeasurementAsync(Guid sensorNodeId, MeasurementDto measurement);
        Task<IEnumerable<MeasurementDto>> GetMeasurementsAsync(Guid sensorNodeId, DateTime? from = null, DateTime? to = null);
    }
}