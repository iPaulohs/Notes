using FluentValidation;
using Notes.DataTransfer.Input.CollectionDataTransfer;

namespace Notes.Validations.Update;

public class CollectionUpdateValidator : AbstractValidator<CollectionInputUpdate>
{
    public CollectionUpdateValidator()
    {
        RuleFor(p => p.Title)
            .NotEmpty()
            .WithMessage("O título não pode estar vazio.")
            .MaximumLength(50);

        RuleFor(p => p.Description)
            .MaximumLength(144)
            .WithMessage("O máximo de caracteres permitido para a descrição é 144.");
    }
}
