using ChatApp.Models;

namespace ChatApp.Services
{
    public interface IUserService
    {
        Task<User> GetUserByIdAsync(int id);
        Task<User> GetUserByUsernameAsync(string username);
        Task<IEnumerable<User>> GetAllUsersAsync();
        Task<bool> UpdateUserStatusAsync(int userId, bool isOnline);
        Task<User> CreateUserAsync(User user);
    }
}