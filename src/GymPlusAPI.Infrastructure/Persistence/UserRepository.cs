using System;
using GymPlusAPI.Domain.Entities;
using GymPlusAPI.Domain.Interfaces;
using GymPlusAPI.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace GymPlusAPI.Infrastructure.Persistence;

public class UserRepository(AppDbContext context) : IUserRepository
{
    public async Task AddAsync(User user)
    {
        if (user == null)
            throw new ArgumentNullException(nameof(user));

        await context.Users.AddAsync(user);
        await context.SaveChangesAsync();
    }

    public Task DeleteAsync(User user)
    {
        if (user == null)
            throw new ArgumentNullException(nameof(user));

        context.Users.Remove(user);
        return context.SaveChangesAsync();
    }

    public async Task<IEnumerable<User>> GetAllUsersAsync() => await context.Users.AsNoTracking().ToListAsync();

    public Task<User?> GetUserByEmailAsync(string email) => context.Users.AsNoTracking().FirstOrDefaultAsync(u => u.Username == email);

    public async Task<User?> GetUserByIdAsync(Guid id) => await context.Users.AsNoTracking().FirstOrDefaultAsync(u => u.Id == id);

    public Task UpdateAsync(User user)
    {
        if (user == null)
            throw new ArgumentNullException(nameof(user));

        context.Users.Update(user);
        return context.SaveChangesAsync();
    }
}
