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
            .WithMessage("O nome é obrigatório.")
            .MinimumLength(5)
            .WithMessage("O nome de uma planilha deve ter mais de 5 caracteres.");
    }
}
