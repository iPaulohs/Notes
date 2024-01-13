using FluentValidation;
using Notes.DataTransfer.Input.NoteDataTransferInput;

namespace Notes.Validations.Input;

public class NoteInputValidator : AbstractValidator<NoteInputInclude>
{
    public NoteInputValidator()
    {
        RuleFor(p => p.Title)
            .NotEmpty()
            .WithMessage("O título não pode estar vazio.")
            .MaximumLength(50);

        RuleFor(p => p.Description)
            .MaximumLength(144)
            .WithMessage("O máximo de caracteres permitido para a descrição é 144.");

        RuleFor(p => p.CollectionId)
            .NotEmpty()
            .GreaterThan(0);

        RuleFor(p => p.FinalDate)
            .NotNull()
            .WithMessage("A data final não pode estar vazia.")
            .Must(NonRetroactiveData)
            .WithMessage("A data final não pode ser retroativa.");
    }

    public bool NonRetroactiveData(DateTime? finalDate)
    {
        return finalDate >= DateTime.UtcNow;
    }
}
