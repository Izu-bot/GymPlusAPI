namespace GymPlusAPI.Domain.Exceptions;

public class UserExistsException(string email)
    : DomainException($"Esse email \'{email}\' já esta cadastrado.");