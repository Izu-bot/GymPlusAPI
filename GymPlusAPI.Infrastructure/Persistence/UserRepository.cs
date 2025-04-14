using System;
using GymPlusAPI.Domain.Entities;
using GymPlusAPI.Domain.Interfaces;
using GymPlusAPI.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace GymPlusAPI.Infrastructure.Persistence;

public class UserRepository : IUserRepository
{
    private readonly AppDbContext _context;

    public UserRepository(AppDbContext context) => _context = context;

    public async Task AddAsync(User user)
    {
        if (user == null)
        {
            throw new ArgumentNullException(nameof(user));
        }

        await _context.Users.AddAsync(user);
        await _context.SaveChangesAsync();
    }

    public Task DeleteAsync(User user)
    {
        if (user == null)
        {
            throw new ArgumentNullException(nameof(user));
        }

        _context.Users.Remove(user);
        return _context.SaveChangesAsync();
    }

    public async Task<IEnumerable<User>> GetAllUsersAsync() => await _context.Users.AsNoTracking().ToListAsync();

    public async Task<User?> GetUserByIdAsync(Guid id) => await _context.Users.AsNoTracking().FirstOrDefaultAsync(u => u.Id == id);

    public Task UpdateAsync(User user)
    {
        if (user == null)
        {
            throw new ArgumentNullException(nameof(user));
        }

        _context.Users.Update(user);
        return _context.SaveChangesAsync();
    }
}
