namespace SUE.Data.Entities;

public class SensorNode
{
    public Guid Id { get; set; }

    public string NodeName { get; set; } = null!;
    public string SensorTypes { get; set; } = null!; 
    public bool IsOnline { get; set; } = true;

    public Guid HouseholdId { get; set; }
    public Household Household { get; set; } = null!;
    
    public enum SensorLocation { Indoor, Outdoor }
    public SensorLocation Location { get; set; }

    public string SensorName { get; set; } = null!;
    public string SensorModel { get; set; } = null!;


    public List<Measurement> Measurements { get; set; } = new();
}