using System;
namespace AuthenticationService.Core.DTOs.AuthDTOs
{
	public class RegistrationDTO
	{
		public string FirstName { get; set; } = string.Empty;

        public string LastName { get; set; } = string.Empty;

        public string Email { get; set; } = string.Empty;

        public AddressDTO? Address { get; set; }

        public string BVN { get; set; } = string.Empty;

        public string PhoneNumber { get; set; } = string.Empty;

        public string Password { get; set; } = string.Empty;

        public string ConfirmPassword { get; set; } = string.Empty;

        public string Pin { get; set; } = string.Empty;

    }
}

