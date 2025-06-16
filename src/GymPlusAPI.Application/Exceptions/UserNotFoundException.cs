namespace GymPlusAPI.Application.Exceptions;

public class UserNotFoundException(string email) 
    : ApplicationException($"Usuário com esse email '{email}' não encontrado");