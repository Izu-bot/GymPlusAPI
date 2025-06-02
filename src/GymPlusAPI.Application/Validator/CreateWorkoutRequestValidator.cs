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
            .WithMessage("Name is required.")
            .MinimumLength(5)
            .WithMessage("Name must be at least 5 characters long.");

        RuleFor(w => w.Reps)
            .NotEmpty()
            .WithMessage("Reps is required.")
            .GreaterThan(0)
            .WithMessage("Reps must be greater than 0.");

        RuleFor(w => w.Series)
            .NotEmpty()
            .WithMessage("Reps is required.")
            .GreaterThan(0)
            .WithMessage("Reps must be greater than 0.");

        RuleFor(w => w.Weight)
            .NotEmpty()
            .WithMessage("Weight is required.")
            .GreaterThan(0)
            .WithMessage("Weight must be greater than 0.");

        RuleFor(w => w.SpreadsheetId)
            .NotEmpty()
            .WithMessage("SpreadsheetId is required.")
            .GreaterThan(0)
            .WithMessage("The workout must be associated with a workout sheet greater than 0");
    }
}
