using System;
using AuthenticationService.Core.DTOs.AuthDTOs;
using FluentValidation;

namespace AuthenticationService.Core.Utilities
{
	public class LoginUserValidator : AbstractValidator<LoginDTO>
	{
		public LoginUserValidator()
		{
			RuleFor(LoginDTO => LoginDTO.Email).NotEmpty().WithMessage("Email must not be empty").EmailAddress();
            RuleFor(LoginDTO => LoginDTO.Password).NotEmpty().WithMessage("Password must not be empty").Password();
        }
    }
}

