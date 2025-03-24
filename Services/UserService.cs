using ChatApp.DTOs;
using ChatApp.Services.interfaces;
namespace ChatApp.Services;

public class UserService(HttpClient httpClient, IConfiguration configuration) : IUserService
{
    public async Task<UserDto?> GetUserByIdAsync(Guid userId)
    {
        string userServiceUrl = configuration["UserService:BaseUrl"];
        var response = await httpClient.GetAsync($"{userServiceUrl}/users/{userId}");

        if (!response.IsSuccessStatusCode)
            return null;

        return await response.Content.ReadFromJsonAsync<UserDto>();
    }
}


