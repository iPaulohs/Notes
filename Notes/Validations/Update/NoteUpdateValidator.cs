using FluentValidation;
using Notes.DataTransfer.Input.NoteDataTransferInput;

namespace Notes.Validations.Update;

public class NoteUpdateValidator : AbstractValidator<NoteInputUpdate>
{
    public NoteUpdateValidator()
    {
        RuleFor(p => p.Title)
             .NotEmpty()
             .WithMessage("O título não pode estar vazio.")
             .MaximumLength(50)
             .WithMessage("O título deve ter no máximo 50 caracteres.");

        RuleFor(p => p.Description)
            .MaximumLength(144)
            .WithMessage("A descrição deve ter no máximo 144 caracteres.");

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
