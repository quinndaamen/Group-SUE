using System;

namespace SUE.Services.Users.Models
{
    public class UserProfileDto
    {
        public Guid UserId { get; set; }
        public string FullName { get; set; } = null!;
        public Guid HouseholdId { get; set; }
    }
}