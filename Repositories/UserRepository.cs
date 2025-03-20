using ChatApp.DataAccess;
using ChatApp.Models;
using ChatApp.Repositories.interfaces;
using Microsoft.EntityFrameworkCore;

namespace ChatApp.Repositories;

public class UserRepository : IUserRepository
{
    private readonly ChatDbContext _context;

    public UserRepository(ChatDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<User>> GetAllUsersAsync()
    {
        return await _context.Users.ToListAsync();
    }

    public async Task<User> GetUserByIdAsync(Guid id)
    {
        return await _context.Users.FindAsync(id);
    }
    
    public async Task<User> CreateUserAsync(User user)
    {
        await _context.Users.AddAsync(user);
        await _context.SaveChangesAsync();
        return user;
    }

    public async Task<User> UpdateUserAsync(User user)
    {
        var existingUser = await _context.Users.FindAsync(user.Id);
        if (existingUser == null) return null;

        _context.Entry(existingUser).CurrentValues.SetValues(user);
        await _context.SaveChangesAsync();
        return existingUser;
    }

    public async Task<bool> DeleteUserByIdAsync(Guid id)
    {
        var user = await _context.Users.FindAsync(id);
        if (user == null) return false;

        _context.Users.Remove(user);
        await _context.SaveChangesAsync();
        return true;
    }

}
