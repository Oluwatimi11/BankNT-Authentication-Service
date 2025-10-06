using FluentValidation;
using AuthenticationService.Core.DTOs.PasswordDTOs;

namespace AuthenticationService.Core.Utilities
{
    public class ResetPasswordValidator : AbstractValidator<ResetPasswordDTO>
    {
        public ResetPasswordValidator()
        {
            RuleFor(ResetPasswordDTO => ResetPasswordDTO.Email).EmailAddress().WithMessage("Invalid Email").NotEmpty().NotNull();
            RuleFor(ResetPasswordDTO => ResetPasswordDTO.NewPassword).Password();
            RuleFor(ResetPasswordDTO => ResetPasswordDTO.ConfirmPassword.Equals(ResetPasswordDTO.NewPassword));
        }
    }
}
