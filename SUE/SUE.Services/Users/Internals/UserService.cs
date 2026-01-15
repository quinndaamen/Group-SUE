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
    public class UserService : IUserService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly AppDbContext _context;

        public UserService(UserManager<ApplicationUser> userManager, AppDbContext context)
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
            {
                var errors = string.Join(", ", result.Errors.Select(e => e.Description));
                throw new UserRegistrationException($"Failed to create user: {errors}");
            }

            // 2️⃣ Transaction for household, profile, and sensors
            using var transaction = await _context.Database.BeginTransactionAsync();

            // 3️⃣ Find or create Household (based on P1ID + PostalCode)
            var household = await _context.Households
                .FirstOrDefaultAsync(h => h.P1ID == model.P1ID && h.PostalCode == model.PostalCode);

            bool isNewHousehold = false;

            if (household == null)
            {
                household = new Household
                {
                    Id = Guid.NewGuid(),
                    PostalCode = model.PostalCode,
                    HouseholdSize = 1,
                    P1ID = model.P1ID
                };
                _context.Households.Add(household);
                isNewHousehold = true;
            }
            else
            {
                household.HouseholdSize++;
                _context.Households.Update(household);
            }

            // 4️⃣ Create UserProfile
            var profile = new UserProfile
            {
                ApplicationUserId = user.Id,
                HouseholdId = household.Id,
                FirstName = model.FirstName,
                LastName = model.LastName
            };
            _context.UserProfiles.Add(profile);

            // 5️⃣ Seed indoor SensorNode only
            if (isNewHousehold)
            {
                var indoorSensor = new SensorNode
                {
                    Id = Guid.NewGuid(),
                    HouseholdId = household.Id,
                    Location = SensorNode.SensorLocation.Indoor,
                    SensorName = "Indoor Node 1"
                };
                _context.SensorNodes.Add(indoorSensor);
            }

            // 6️⃣ Commit everything
            await _context.SaveChangesAsync();
            await transaction.CommitAsync();

            // 7️⃣ Return DTO
            return new UserProfileDto
            {
                UserId = user.Id,
                FullName = $"{model.FirstName} {model.LastName}",
                HouseholdId = household.Id
            };
        }
    }

    public class UserRegistrationException : Exception
    {
        public UserRegistrationException(string message) : base(message) { }
    }
}
