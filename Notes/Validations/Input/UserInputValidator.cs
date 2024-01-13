using FluentValidation;
using Notes.DataTransfer.Input.UserDataTransferInput;

namespace Notes.Validations.Input;

public class UserInputValidator : AbstractValidator<UserInputRegister>
{
    public UserInputValidator()
    {
        RuleFor(p => p.Email)
            .NotEmpty()
            .WithMessage("O Email não pode estar vazio.")
            .EmailAddress();

        RuleFor(p => p.UserName)
            .NotEmpty()
            .WithMessage("O Username não pode estar vazio.")
            .MinimumLength(5);

        RuleFor(p => p.Name)
            .NotEmpty()
            .WithMessage("O Nome não pode estar vazio.")
            .Length(2, 80);

        RuleFor(p => p.Password)
            .NotEmpty()
            .WithMessage("O Password não pode estar vazio.")
            .MinimumLength(6);
    }
}
