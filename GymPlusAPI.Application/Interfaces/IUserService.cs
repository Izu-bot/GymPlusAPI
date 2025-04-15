using System;
using GymPlusAPI.Application.DTOs.User;

namespace GymPlusAPI.Application.Interfaces;

public interface IUserService
{
    Task<Guid> AddAsync(UserCreateDTO dto);
    Task UpdateAsync(UserUpdateDTO dto);
    Task DeleteAsync(Guid id);
    Task <UserViewDTO?> GetByIdAsync(Guid id);
    Task<UserViewDTO?> GetByEmailAsync(string email);
    Task<IEnumerable<UserViewDTO>> GetAllAsync();
}
