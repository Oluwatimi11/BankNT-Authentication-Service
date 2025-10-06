using System;
using FluentValidation;

namespace AuthenticationService.Core.DTOs.EmailDTOs
{
	public class ConfirmEmailDTO
	{
		public string EmailAddress { get; set; } = string.Empty;

        public string Token { get; set; } = string.Empty;


    }

	public class ConfirmEmailValidator : AbstractValidator<ConfirmEmailDTO>
	{
		public ConfirmEmailValidator()
		{
			RuleFor(x => x.EmailAddress).NotEmpty().WithMessage("Email address is required").EmailAddress()
.WithMessage("A valid email is required");
			RuleFor(x => x.Token).NotEmpty().WithMessage("Token is required");
		}
	}
}

