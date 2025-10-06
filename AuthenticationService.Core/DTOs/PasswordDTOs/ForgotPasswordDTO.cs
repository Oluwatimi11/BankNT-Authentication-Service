using System;
using FluentValidation;

namespace AuthenticationService.Core.DTOs.PasswordDTOs
{
	public class ForgotPasswordDTO
	{
		public string EmailAddress { get; set; } = string.Empty;
	}

	public class EmailValidator : AbstractValidator<ForgotPasswordDTO>
	{
		public EmailValidator()
		{ 
			RuleFor(s => s.EmailAddress).NotEmpty().EmailAddress();
		}
	}
}

