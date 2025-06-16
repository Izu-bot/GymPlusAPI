using FluentValidation;
using GymPlusAPI.Application.DTOs.Request.Login;

namespace GymPlusAPI.Application.Validator;

public class LoginRequestValidator : AbstractValidator<LoginRequest>
{
    public LoginRequestValidator()
    {
        RuleFor(l => l.Email)
            .NotEmpty()
            .WithMessage("O campo de email não deve ficar vazio.")
            .EmailAddress()
            .WithMessage("Insira um endereço de email válido.");

        RuleFor(l => l.Password)
            .NotEmpty()
            .WithMessage("O campo da senha não deve ficar vazio.");
    }
}