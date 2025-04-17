using System;
using GymPlusAPI.Application.DTOs.Request.User;
using GymPlusAPI.Application.DTOs.Response.User;
using GymPlusAPI.Application.Interfaces;
using GymPlusAPI.Application.Validator;
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

    public async Task<UserResponse> AddAsync(CreateUserRequest dto)
    {
        Validate(dto); // Valida com Fluent Validation
        var existingUser = await _userRepository.GetUserByEmailAsync(dto.Email);

        if (existingUser != null)
            throw new Exception("Usuário já existe.");

        var hashedPassword = _passwordHasher.HashPassword(dto.Password);

        var newUser = new User(dto.Email, hashedPassword, dto.Name, dto.Role);

        await _userRepository.AddAsync(newUser);

        return new UserResponse
        (
            newUser.Id,
            newUser.Username,
            newUser.Name
        );
    }

    public async Task DeleteAsync(Guid id)
    {
        var userToDelete = await _userRepository.GetUserByIdAsync(id);

        if (userToDelete == null)
            throw new Exception("Usuário não encontrado.");

        await _userRepository.DeleteAsync(userToDelete);
    }

    public async Task<IEnumerable<UserResponse>> GetAllAsync()
    {
        var users = await _userRepository.GetAllUsersAsync();

        return users.Select(u => new UserResponse
        (
            u.Id,
            u.Username,
            u.Name
        )).ToList();
    }

    public async Task<UserResponse?> GetByEmailAsync(string email)
    {
        var user = await _userRepository.GetUserByEmailAsync(email);
        if (user == null)
            return null!;

        return new UserResponse
        (
            user.Id,
            user.Username,
            user.Name
        );
    }

    public async Task<UserResponse?> GetByIdAsync(Guid id)
    {
        var user = await _userRepository.GetUserByIdAsync(id);
        if (user == null)
            return null!;

        return new UserResponse
        (
            user.Id,
            user.Username,
            user.Name
        );
    }

public async Task UpdateAsync(UpdateUserRequest dto)
{
    var userToUpdate = await _userRepository.GetUserByIdAsync(dto.Id);

    if (userToUpdate == null)
        throw new Exception("Usuário não encontrado.");

    bool hasChanges = false;

    if (!string.IsNullOrWhiteSpace(dto.Email) && dto.Email != userToUpdate.Username)
    {
        userToUpdate.Username = dto.Email;
        hasChanges = true;
    }

    if (!string.IsNullOrWhiteSpace(dto.Password))
    {
        userToUpdate.Password = _passwordHasher.HashPassword(dto.Password); // se você tiver hashing
        hasChanges = true;
    }

    if (!string.IsNullOrWhiteSpace(dto.Name) && dto.Name != userToUpdate.Name)
    {
        userToUpdate.Name = dto.Name;
        hasChanges = true;
    }

    if (hasChanges)
    {
        await _userRepository.UpdateAsync(userToUpdate);
    }
}

    private static void Validate(CreateUserRequest request)
    {
        var validate = new CreateUserRequestValidator();

        var result = validate.Validate(request);
        if (!result.IsValid)
        {
            throw new Exception(result.Errors.First().ErrorMessage);
        }
    }
}
