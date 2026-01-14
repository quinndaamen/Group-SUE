using System;
using System.Collections.Generic;
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
        _serial.NewLine = "\n";
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
        var measurements = new List<MeasurementDto>();
        var currentMeasurement = new MeasurementDto();

        while (_serial.BytesToRead > 0)
        {
            var line = _serial.ReadLine().Trim();
            if (string.IsNullOrEmpty(line)) continue;

            if (line == "!")
            {
                if (currentMeasurement.HasAnyData())
                {
                    measurements.Add(currentMeasurement);
                    currentMeasurement = new MeasurementDto();
                }
                continue;
            }

            try
            {
                // Total energy usage (kWh)
                if (line.StartsWith("1-0:1.8.0"))
                {
                    var start = line.IndexOf('(') + 1;
                    var end = line.IndexOf('*');
                    currentMeasurement.EnergyTotalKWh = double.Parse(line[start..end]);
                }
                // Current power (kW)
                else if (line.StartsWith("1-0:1.7.0"))
                {
                    var start = line.IndexOf('(') + 1;
                    var end = line.IndexOf('*');
                    currentMeasurement.CurrentPowerKW = double.Parse(line[start..end]);
                }
                // Temperature / Humidity / AQI
                else if (line.StartsWith("Temp:"))
                {
                    var parts = line.Split(';', StringSplitOptions.RemoveEmptyEntries);
                    foreach (var part in parts)
                    {
                        var kv = part.Split(':', StringSplitOptions.RemoveEmptyEntries);
                        if (kv.Length != 2) continue;

                        var key = kv[0].Trim();
                        var value = kv[1].Trim();

                        if (key == "Temp") currentMeasurement.Temperature = double.Parse(value);
                        else if (key == "Humid") currentMeasurement.Humidity = double.Parse(value);
                        else if (key == "Sensor") currentMeasurement.AQI = double.Parse(value);
                    }
                }
            }
            catch
            {
                // ignore malformed lines
                continue;
            }
        }

        if (currentMeasurement.HasAnyData())
            measurements.Add(currentMeasurement);

        return measurements;
    }
}

// Extension to check if a MeasurementDto has any data
public static class MeasurementDtoExtensions
{
    public static bool HasAnyData(this MeasurementDto m)
    {
        return m.EnergyTotalKWh.HasValue ||
               m.CurrentPowerKW.HasValue ||
               m.Temperature.HasValue ||
               m.Humidity.HasValue ||
               m.AQI.HasValue;
    }
}
