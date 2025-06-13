using GymPlusAPI.Application.DTOs.Request.User;
using GymPlusAPI.Application.DTOs.Response.User;
using GymPlusAPI.Application.Interfaces;
using GymPlusAPI.Application.Validator;
using GymPlusAPI.Domain.Entities;
using GymPlusAPI.Domain.Exceptions;
using GymPlusAPI.Domain.Interfaces;

namespace GymPlusAPI.Application.Services;

public class UserService(IUserRepository userRepository, IPasswordHasher passwordHasher)
    : IUserService
{
    public async Task<UserResponse> AddAsync(CreateUserRequest dto)
    {
        Validate(dto);
        var existingUser = await userRepository.GetUserByEmailAsync(dto.Email);

        if (existingUser != null)
            throw new UserExistsException(existingUser.Username);

        var hashedPassword = passwordHasher.HashPassword(dto.Password);

        var newUser = new User(dto.Email, hashedPassword, dto.Name, dto.Role);

        await userRepository.AddAsync(newUser);

        return new UserResponse
        (
            newUser.Id,
            newUser.Username,
            newUser.Name
        );
    }

    public async Task DeleteAsync(Guid id)
    {
        var userToDelete = await userRepository.GetUserByIdAsync(id) ?? throw new EntityNotFoundException("Este usuário");

        await userRepository.DeleteAsync(userToDelete);
    }

    public async Task<IEnumerable<UserResponse>> GetAllAsync()
    {
        var users = (await userRepository.GetAllUsersAsync()).ToList();

        if (!users.Any()) throw new EntityNotFoundException("Não há nenhum Usuário cadastrado.");

        return users.Select(u => new UserResponse
        (
            u.Id,
            u.Username,
            u.Name
        )).ToList();
    }

    public async Task<UserResponse> GetByIdAsync(Guid id)
    {
        var user = await userRepository.GetUserByIdAsync(id);
        if (user == null)
            throw new EntityNotFoundException("Esse Usuário não existe.");

        return new UserResponse
        (
            user.Id,
            user.Username,
            user.Name
        );
    }

public async Task UpdateAsync(UpdateUserRequest dto)
{
    var userToUpdate = await userRepository.GetUserByIdAsync(dto.Id);

    if (userToUpdate == null)
        throw new EntityNotFoundException("Usuário não encontrado.");

    bool hasChanges = false;

    if (!string.IsNullOrWhiteSpace(dto.Email) && dto.Email != userToUpdate.Username)
    {
        userToUpdate.Username = dto.Email;
        hasChanges = true;
    }

    if (!string.IsNullOrWhiteSpace(dto.Password))
    {
        userToUpdate.Password = passwordHasher.HashPassword(dto.Password); // se você tiver hashing
        hasChanges = true;
    }

    if (!string.IsNullOrWhiteSpace(dto.Name) && dto.Name != userToUpdate.Name)
    {
        userToUpdate.Name = dto.Name;
        hasChanges = true;
    }

    if (hasChanges)
    {
        await userRepository.UpdateAsync(userToUpdate);
    }
}

    private static void Validate(CreateUserRequest request)
    {
        var validate = new CreateUserRequestValidator();

        var result = validate.Validate(request);
        if (!result.IsValid)
        {
            throw new ValidationException(result.Errors.First().ErrorMessage);
        }
    }
}
