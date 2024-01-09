using FluentValidation;
using Notes.DataTransfer.Input.CollectionDataTransfer;

namespace Notes.Validations;

public class CollectionInputValidator : AbstractValidator<CollectionInputInclude>
{
    public CollectionInputValidator() 
    {
        RuleFor(p => p.Title).NotEmpty().MaximumLength(50).NotNull().Matches("^[a-zA-Z0-9 ]*$");
        RuleFor(p => p.Description).MaximumLength(144);
    }
}
