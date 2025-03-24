using ChatApp.DataAccess;
using ChatApp.Models;
using ChatApp.Repositories.interfaces;
using Microsoft.EntityFrameworkCore;

namespace ChatApp.Repositories;

public class UserRepository(ChatDbContext context) : IUserRepository
{
    public async Task<IEnumerable<User>> GetAllUsersAsync()
    {
        return await context.Users.ToListAsync();
    }

    public async Task<User?> GetUserByIdAsync(Guid id)
    {
        return await context.Users.FindAsync(id);
    }
    
    public async Task<User> CreateUserAsync(User user)
    {
        await context.Users.AddAsync(user);
        await context.SaveChangesAsync();
        return user;
    }

    public async Task<User?> UpdateUserAsync(User user)
    {
        var existingUser = await context.Users.FindAsync(user.Id);
        if (existingUser == null) return null;

        context.Entry(existingUser).CurrentValues.SetValues(user);
        await context.SaveChangesAsync();
        return existingUser;
    }

    public async Task<bool> DeleteUserByIdAsync(Guid id)
    {
        var user = await context.Users.FindAsync(id);
        if (user == null) return false;

        context.Users.Remove(user);
        await context.SaveChangesAsync();
        return true;
    }

}
