using System;
using FluentValidation;
using GymPlusAPI.Application.DTOs.Request.Spreadsheet;
using Microsoft.VisualBasic.CompilerServices;

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
        
        RuleFor(x => x.Description)
            .MaximumLength(100)
            .WithMessage("A descrição deve ter no máximo 100 caracteres.");

        RuleFor(x => x)
            .Custom((dto, context) =>
            {
                if (dto.IsRecurring && dto.DaysOfWeek.Count == 0)
                    context.AddFailure("Selecione pelo menos um dia da semana.");

                if (dto.DaysOfWeek.Any() && dto.IsRecurring == false)
                    context.AddFailure("Marque o exercicio como recorrente para selecionar os dias da semana.");
            });

    }
}