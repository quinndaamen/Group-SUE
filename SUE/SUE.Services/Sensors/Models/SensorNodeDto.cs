using System;

namespace SUE.Services.Sensors.Models
{
    public class SensorNodeDto
    {
        public Guid Id { get; set; }
        public Guid HouseholdId { get; set; }
        public string SensorName { get; set; } = null!;
        public string Location { get; set; } = null!;
    }
}