using FluentValidation;
using GymPlusAPI.Application.DTOs.Request.Spreadsheet;

namespace GymPlusAPI.Application.Validator;

public class UpdateSpreadsheetValidator : AbstractValidator<UpdateSpreadsheetRequest>
{
    public UpdateSpreadsheetValidator()
    {
        RuleFor(u => u.Id)
            .NotEmpty()
            .WithMessage("Associe a uma planilha que exista.");
        
        RuleFor(u => u.Name)
            .NotEmpty()
            .WithMessage("O nome da planilha deve ser informado.")
            .MinimumLength(5);
    }
}