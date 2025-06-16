namespace GymPlusAPI.Domain.Exceptions;

public class EntityNotFoundException(string entity) 
    : DomainException($"Essa entidade '{entity}' não existe no sistema.");