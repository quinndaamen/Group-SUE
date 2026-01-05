using System.Net.NetworkInformation;

namespace SUE.Data.Entities;

public class UserProfile
{
    
    public Guid ApplicationUserId { get; set; }
    public ApplicationUser ApplicationUser { get; set; }
    
    public string FirstName { get; set; }
    public string LastName { get; set; }
    
    public string Email { get; set; }
    public string PhoneNumber { get; set; }
    public string PostCode { get; set; }
    
    
}