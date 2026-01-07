namespace SUE.Services.Users.Models
{
    public class RegisterModel
    {
        public string Email { get; set; } = null!;
        public string Password { get; set; } = null!;
        public string FirstName { get; set; } = null!;
        public string LastName { get; set; } = null!;
        public string PostalCode { get; set; } = null!;
        public string RegionCode { get; set; } = null!;
    }
}