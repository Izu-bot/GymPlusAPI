using FluentValidation;
using GymPlusAPI.Application.DTOs.Request.RecurrentTraining;

namespace GymPlusAPI.Application.Validator;

public class CreateRecurrentTrainingRequestValidator : AbstractValidator<RecurrentTrainingRequest>
{
    public CreateRecurrentTrainingRequestValidator()
    {
        RuleFor(rtr => rtr.IsCompleted)
            .NotNull()
            .WithMessage("Não deixe o campo de treino completo em branco.")
            .Must(x => x)
            .WithMessage("Você deve marcar o treino como completo.");

        RuleFor(rtr => rtr.Description)
            .MinimumLength(10)
            .WithMessage("A descrição deve ter no minimo 10 caracteres")
            .MaximumLength(150)
            .WithMessage("A descrição deve ter no máximo 150 caracteres");
    }
}