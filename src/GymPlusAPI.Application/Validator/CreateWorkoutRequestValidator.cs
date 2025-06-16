using System;
using FluentValidation;
using GymPlusAPI.Application.DTOs.Request.Workout;

namespace GymPlusAPI.Application.Validator;

public class CreateWorkoutRequestValidator : AbstractValidator<CreateWorkoutRequest>
{
    public CreateWorkoutRequestValidator()
    {
        RuleFor(w => w.Name)
            .NotEmpty()
            .WithMessage("O nome é obrigatório.")
            .MinimumLength(5)
            .WithMessage("O nome deve possuir no minimo 5 caracteres.");

        RuleFor(w => w.Reps)
            .NotEmpty()
            .WithMessage("Números de repetição é obrigátorio.")
            .GreaterThan(0)
            .WithMessage("As repetições deve ser maior que 0.");

        RuleFor(w => w.Series)
            .NotEmpty()
            .WithMessage("O número de series é obrigatório.")
            .GreaterThan(0)
            .WithMessage("As series deve ser maior que 0.");

        RuleFor(w => w.Weight)
            .NotEmpty()
            .WithMessage("O peso é obrigatório.")
            .GreaterThan(0)
            .WithMessage("O peso deve ser maior que 0.");

        RuleFor(w => w.SpreadsheetId)
            .NotEmpty()
            .WithMessage("Você deve associar o treino a uma planilha.")
            .GreaterThan(0)
            .WithMessage("Associe a um treio que exista.");
    }
}
