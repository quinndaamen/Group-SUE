using SUE.Services.Users.Models;
using System.Threading.Tasks;

namespace SUE.Services.Users.Contracts
{
    public interface IUserOnboardingService
    {
        Task<UserProfileDto> RegisterUserAsync(RegisterModel model);
    }
}