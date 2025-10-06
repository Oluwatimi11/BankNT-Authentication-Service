using System;
namespace AuthenticationService.Core.DTOs
{
	public class ResendOtpDTO
	{
		public string Email { get; set; } = string.Empty;

		public string Purpose { get; set; } = string.Empty;
	}
}

