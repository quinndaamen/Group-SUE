using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using SUE.Data;
using SUE.Data.Entities;
using SUE.Services.Users.Contracts;
using SUE.Services.Users.Models;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace SUE.Services.Users.Internals
{
    public class UserOnboardingService : IUserOnboardingService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly AppDbContext _context;

        public UserOnboardingService(UserManager<ApplicationUser> userManager, AppDbContext context)
        {
            _userManager = userManager;
            _context = context;
        }

        public async Task<UserProfileDto> RegisterUserAsync(RegisterModel model)
        {
            // 1️⃣ Create ApplicationUser
            var user = new ApplicationUser
            {
                UserName = model.Email,
                Email = model.Email
            };
            var result = await _userManager.CreateAsync(user, model.Password);
            if (!result.Succeeded)
                throw new Exception(string.Join(", ", result.Errors.Select(e => e.Description)));

            // 2️⃣ Find or create Household
            var household = await _context.Households
                .FirstOrDefaultAsync(h => h.PostalCode == model.PostalCode);

            if (household == null)
            {
                household = new Household
                {
                    Id = Guid.NewGuid(),
                    PostalCode = model.PostalCode,
                    RegionCode = model.RegionCode,
                    HouseholdSize = 1
                };
                _context.Households.Add(household);
            }
            else
            {
                household.HouseholdSize++;
            }

            await _context.SaveChangesAsync();

            // 3️⃣ Create UserProfile
            var profile = new UserProfile
            {
                ApplicationUserId = user.Id,
                HouseholdId = household.Id,
                FirstName = model.FirstName,
                LastName = model.LastName
            };
            _context.UserProfiles.Add(profile);

            // 4️⃣ Seed SensorNodes (indoor/outdoor)
            var indoorNode = new SensorNode
            {
                Id = Guid.NewGuid(),
                HouseholdId = household.Id,
                Location = SensorNode.SensorLocation.Indoor,
                SensorName = "Indoor Node 1"
            };
            var outdoorNode = new SensorNode
            {
                Id = Guid.NewGuid(),
                HouseholdId = household.Id,
                Location = SensorNode.SensorLocation.Outdoor,
                SensorName = "Outdoor Node 1"
            };
            _context.SensorNodes.AddRange(indoorNode, outdoorNode);

            await _context.SaveChangesAsync();

            return new UserProfileDto
            {
                UserId = user.Id,
                FullName = $"{model.FirstName} {model.LastName}",
                HouseholdId = household.Id
            };
        }
    }
}
