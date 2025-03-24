using ChatApp.Models;
using ChatApp.Repositories.interfaces;
using ChatApp.Services.interfaces;

namespace ChatApp.Services;

public class UserService(IUserRepository userRepository) : IUserService
{
    public async Task<IEnumerable<User>> GetAllUsersAsync()
    {
        return await userRepository.GetAllUsersAsync();
    }

    public async Task<User?> GetUserByIdAsync(Guid id)
    {
        return await userRepository.GetUserByIdAsync(id);
    }

    public async Task<User> CreateUserAsync(User user)
    {
        return await userRepository.CreateUserAsync(user);
    }

    public async Task<User?> UpdateUserAsync(User user)
    {
        return await userRepository.UpdateUserAsync(user);
    }

    public async Task<bool> DeleteUserByIdAsync(Guid id)
    {
        return await userRepository.DeleteUserByIdAsync(id);
    }
}
