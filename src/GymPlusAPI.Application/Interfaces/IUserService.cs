using GymPlusAPI.Application.DTOs.Request.User;
using GymPlusAPI.Application.DTOs.Response.User;

namespace GymPlusAPI.Application.Interfaces;

public interface IUserService
{
    Task<UserResponse> AddAsync(CreateUserRequest dto);
    Task UpdateAsync(UpdateUserRequest dto);
    Task DeleteAsync(Guid id);
    Task <UserResponse> GetByIdAsync(Guid id);
    Task<IEnumerable<UserResponse>> GetAllAsync();
}
