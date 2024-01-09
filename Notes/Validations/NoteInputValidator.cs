using FluentValidation;
using Notes.DataTransfer.Input.NoteDataTransferInput;

namespace Notes.Validations;

public class NoteInputValidator : AbstractValidator<NoteInputInclude>
{
    public NoteInputValidator()
    {
        RuleFor(p => p.Title).NotEmpty().MaximumLength(50).NotNull();
        RuleFor(p => p.Description).MaximumLength(144);
        RuleFor(p => p.CollectionId).NotEmpty().GreaterThan(0);
    }
}
