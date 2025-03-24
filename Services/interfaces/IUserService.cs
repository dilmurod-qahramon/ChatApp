using ChatApp.DTOs;

namespace ChatApp.Services.interfaces;
public interface IUserService
{
    Task<UserDto?> GetUserByIdAsync(Guid userId);
}
