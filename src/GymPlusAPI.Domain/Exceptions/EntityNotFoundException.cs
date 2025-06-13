namespace GymPlusAPI.Domain.Exceptions;

public class EntityNotFoundException(string entity) 
    : DomainException($"Infelizmente {entity}");