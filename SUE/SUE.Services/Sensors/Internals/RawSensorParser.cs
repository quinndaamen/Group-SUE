using System.Globalization;
using SUE.Services.Sensors.Models;

public static class RawSensorParser
{
    public static MeasurementDto? Parse(string payload)
    {
        var dto = new MeasurementDto();
        var lines = payload.Split('\n', StringSplitOptions.RemoveEmptyEntries);

        foreach (var raw in lines)
        {
            var line = raw.Trim();

            if (line.Contains("1-0:1.8.0"))
                dto.EnergyTotalKWh = ExtractBetween(line, '(', '*');

            else if (line.Contains("1-0:1.7.0"))
                dto.CurrentPowerKW = ExtractBetween(line, '(', '*');

            else if (line.StartsWith("Temp:"))
                dto.Temperature = ExtractAfterColon(line);

            else if (line.StartsWith("Humid:"))
                dto.Humidity = ExtractAfterColon(line);

            else if (line.StartsWith("Sensor:"))
                dto.AQI = ExtractAfterColon(line);
        }

        if (dto.Temperature == null &&
            dto.Humidity == null &&
            dto.AQI == null &&
            dto.EnergyTotalKWh == null &&
            dto.CurrentPowerKW == null)
            return null;

        return dto;
    }

    private static double? ExtractBetween(string line, char start, char end)
    {
        var s = line.IndexOf(start);
        var e = line.IndexOf(end);
        if (s == -1 || e == -1 || e <= s) return null;

        var value = line.Substring(s + 1, e - s - 1);
        return double.TryParse(value, NumberStyles.Any, CultureInfo.InvariantCulture, out var d)
            ? d
            : null;
    }

    private static double? ExtractAfterColon(string line)
    {
        var idx = line.IndexOf(':');
        if (idx == -1) return null;

        var value = line[(idx + 1)..].Replace(";", "").Trim();
        return double.TryParse(value, NumberStyles.Any, CultureInfo.InvariantCulture, out var d)
            ? d
            : null;
    }
}