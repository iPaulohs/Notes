using FluentValidation;
using Notes.DataTransfer.Input.UserDataTransferInput;

namespace Notes.Validations;

public class UserInputValidator : AbstractValidator<UserInputRegister>
{
    public UserInputValidator() 
    {
        RuleFor(p => p.Email).EmailAddress();
        RuleFor(p => p.UserName).NotEmpty().MinimumLength(5);
        RuleFor(p => p.Name).NotEmpty().Length(2, 50);
        RuleFor(p => p.Password).NotEmpty().MinimumLength(6);
    }
}
