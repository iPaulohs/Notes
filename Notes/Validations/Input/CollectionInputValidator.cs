using FluentValidation;
using Notes.DataTransfer.Input.CollectionDataTransfer;

namespace Notes.Validations.Input;

public class CollectionInputValidator : AbstractValidator<CollectionInputInclude>
{
    public CollectionInputValidator()
    {
        RuleFor(p => p.Title)
            .NotEmpty()
            .WithMessage("O título não pode estar vazio.")
            .MaximumLength(50);

        RuleFor(p => p.Description)
            .MaximumLength(144)
            .WithMessage("A descrição deve ter no máximo 144 caracteres.");
    }
}
