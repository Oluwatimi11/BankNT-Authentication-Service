using System;
namespace AuthenticationService.Core.DTOs
{
	public class CreateWalletDTO
	{
		public string FirstName { get; set; } = string.Empty;

        public string LastName { get; set; } = string.Empty;

        public string UserEmail { get; set; } = string.Empty;

        public string UserId { get; set; } = string.Empty;

        public string Pin { get; set; } = string.Empty;

    }
}

