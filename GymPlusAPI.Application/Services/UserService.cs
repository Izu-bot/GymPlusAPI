using System;
using GymPlusAPI.Application.DTOs.User;
using GymPlusAPI.Application.Interfaces;
using GymPlusAPI.Domain.Entities;
using GymPlusAPI.Domain.Interfaces;

namespace GymPlusAPI.Application.Services;

public class UserService : IUserService
{
    private readonly IUserRepository _userRepository;
    private readonly IPasswordHasher _passwordHasher;
    public UserService(IUserRepository userRepository, IPasswordHasher passwordHasher)
    {
        _userRepository = userRepository;
        _passwordHasher = passwordHasher;
    }

    public async Task<Guid> AddAsync(UserCreateDTO dto)
    {
        if (dto == null)
            throw new ArgumentNullException(nameof(dto));
        
        var existingUser = await _userRepository.GetUserByEmailAsync(dto.Email);

        if (existingUser != null)
            throw new Exception("Usuário já existe.");
        
        var hashedPassword = _passwordHasher.HashPassword(dto.Password);

        var newUser = new User(dto.Email, hashedPassword, dto.Name, dto.Role);

        await _userRepository.AddAsync(newUser);
        return newUser.Id;
    }

    public async Task DeleteAsync(Guid id)
    {
        var userToDelete = await _userRepository.GetUserByIdAsync(id);

        if (userToDelete == null)
            throw new Exception("Usuário não encontrado.");

        await _userRepository.DeleteAsync(userToDelete);
    }

    public async Task<IEnumerable<UserViewDTO>> GetAllAsync()
    {
        var users = await _userRepository.GetAllUsersAsync();

        return users.Select(u => new UserViewDTO
        (
            u.Username,
            u.Password,
            u.Name
        )).ToList();
    }

    public async Task<UserViewDTO?> GetByEmailAsync(string email)
    {
        var user = await _userRepository.GetUserByEmailAsync(email);
        if (user == null)
            return null!;

        return new UserViewDTO
        (
            user.Username,
            user.Password,
            user.Name
        );
    }

    public async Task<UserViewDTO?> GetByIdAsync(Guid id)
    {
        var user = await _userRepository.GetUserByIdAsync(id);
        if (user == null)
            return null!;

        return new UserViewDTO
        (
            user.Username,
            user.Password,
            user.Name
        );
    }

    public async Task UpdateAsync(UserUpdateDTO dto)
    {
        var userToUpdate = await _userRepository.GetUserByIdAsync(dto.Id);

        if (userToUpdate == null)
            throw new Exception("Usuário não encontrado.");

        userToUpdate.Username = dto.Username;
        userToUpdate.Password = dto.Password;
        userToUpdate.Name = dto.Name;

        await _userRepository.UpdateAsync(userToUpdate);
    }
}
