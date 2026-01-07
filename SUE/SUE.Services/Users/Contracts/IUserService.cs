using SUE.Services.Users.Models;
using System.Threading.Tasks;

namespace SUE.Services.Users.Contracts
{
    public interface IUserService
    {
        Task<UserProfileDto> RegisterUserAsync(RegisterModel model);
    }
}