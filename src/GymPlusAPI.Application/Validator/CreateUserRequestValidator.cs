using System;
using FluentValidation;
using GymPlusAPI.Application.DTOs.Request.User;

namespace GymPlusAPI.Application.Validator;

public class CreateUserRequestValidator : AbstractValidator<CreateUserRequest>
{
    public CreateUserRequestValidator()
    {
        RuleFor(u => u.Email)
            .NotEmpty()
            .WithMessage("Email é obrigatório.")
            .EmailAddress()
            .WithMessage("Email no formato inválido.");
        
        RuleFor(u => u.Password)
            .NotEmpty()
            .WithMessage("A senha é obrigatório.");
        When(u => string.IsNullOrEmpty(u.Password) == false, () =>
        {
            RuleFor(u => u.Password)
            .MinimumLength(8)
            .WithMessage("A senha deve contar no minimo 8 caracteres.");
        });

        RuleFor(u => u.Name)
            .NotEmpty()
            .WithMessage("O nome é obrigatório.")
            .MinimumLength(3)
            .WithMessage("O nome deve no minimo conter 3 caracteres.");
    }
}
