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
            .WithMessage("Email is required.")
            .EmailAddress()
            .WithMessage("Invalid email format.");
        
        RuleFor(u => u.Password)
            .NotEmpty()
            .WithMessage("Password is required.");
        When(u => string.IsNullOrEmpty(u.Password) == false, () =>
        {
            RuleFor(u => u.Password)
            .MinimumLength(8)
            .WithMessage("Password must be at least 8 characters long.");
        });

        RuleFor(u => u.Name)
            .NotEmpty()
            .WithMessage("Name is required.")
            .MinimumLength(2)
            .WithMessage("Name must be at least 2 characters long.");
    }
}
