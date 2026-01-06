using System.ComponentModel.DataAnnotations;
using System.Net.NetworkInformation;

namespace SUE.Data.Entities;

public class UserProfile
{
    [Key] // tells EF Core this is the primary key
    public Guid Id { get; set; } = Guid.NewGuid();
    public Guid ApplicationUserId { get; set; }
    public ApplicationUser ApplicationUser { get; set; }
    
    public Guid HouseholdId { get; set; }
    public Household Household { get; set; } = null!;
    
    public string FirstName { get; set; }
    public string LastName { get; set; }
    
    public string Email { get; set; }
    public string PhoneNumber { get; set; }
    public string PostCode { get; set; }
    
    
}