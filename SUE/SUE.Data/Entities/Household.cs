namespace SUE.Data.Entities;

public class Household
{
    public Guid Id { get; set; }

    // Location & energy info
    public string PostalCode { get; set; } = null!;
    
    public string P1ID { get; set; } = null!;

    public int HouseholdSize { get; set; }

    // Navigation
    public List<UserProfile> Residents { get; set; } = new();
    public List<SensorNode> Sensors { get; set; } = new();
}