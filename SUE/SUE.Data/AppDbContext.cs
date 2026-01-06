using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using SUE.Data.Entities;

namespace SUE.Data
{
    public class AppDbContext : IdentityDbContext<ApplicationUser, IdentityRole<Guid>, Guid>
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options)
        { }

        public DbSet<UserProfile> UserProfiles { get; set; }
        public DbSet<Household> Households { get; set; }
        public DbSet<SensorNode> SensorNodes { get; set; }
        public DbSet<Measurement> Measurements { get; set; }
    }
}