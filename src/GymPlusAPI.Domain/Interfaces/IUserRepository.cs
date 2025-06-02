using System;
using GymPlusAPI.Domain.Entities;

namespace GymPlusAPI.Domain.Interfaces;

public interface IUserRepository
{
    Task<User?> GetUserByIdAsync(Guid id);
    Task<IEnumerable<User>> GetAllUsersAsync();
    Task<User?> GetUserByEmailAsync(string email);
    Task AddAsync(User user);
    Task UpdateAsync(User user);
    Task DeleteAsync(User user);
}
