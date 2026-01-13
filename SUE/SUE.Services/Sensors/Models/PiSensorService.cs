using System;
using System.IO.Ports;
using System.Threading.Tasks;
using SUE.Services.Sensors.Contracts;
using SUE.Services.Sensors.Models;

public class PiSensorService : ISensorService
{
    private readonly SerialPort _serial;

    public PiSensorService(string port = "/dev/ttyUSB0", int baud = 9600)
    {
        _serial = new SerialPort(port, baud);
        _serial.Open();
    }

    public Task<SensorNodeDto> CreateSensorNodeAsync(Guid householdId, string name, string location)
    {
        throw new NotImplementedException();
    }

    public Task<IEnumerable<SensorNodeDto>> GetSensorNodesAsync(Guid householdId)
    {
        throw new NotImplementedException();
    }

    public async Task<MeasurementDto> AddMeasurementAsync(Guid sensorNodeId, MeasurementDto measurement)
    {
        // Optionally save to DB
        return measurement;
    }

    public async Task<IEnumerable<MeasurementDto>> GetMeasurementsAsync(Guid sensorNodeId, DateTime? from = null, DateTime? to = null)
    {
        var list = new List<MeasurementDto>();

        while (_serial.BytesToRead > 0)
        {
            var line = _serial.ReadLine();
            try
            {
                var data = System.Text.Json.JsonSerializer.Deserialize<MeasurementDto>(line);
                if (data != null) list.Add(data);
            }
            catch { /* ignore bad lines */ }
        }

        return list;
    }
}