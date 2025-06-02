using System;
using FluentValidation;
using GymPlusAPI.Application.DTOs.Request.Spreadsheet;

namespace GymPlusAPI.Application.Validator;

public class CreateSpreadsheetRequestValidator : AbstractValidator<CreateSpreadsheetRequest>
{
    public CreateSpreadsheetRequestValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty()
            .WithMessage("Name is required.")
            .MinimumLength(5)
            .WithMessage("Name must be at least 5 characters long.");
    }
}
