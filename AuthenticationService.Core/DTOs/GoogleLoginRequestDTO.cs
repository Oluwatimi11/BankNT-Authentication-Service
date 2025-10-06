using System;
namespace AuthenticationService.Core.DTOs
{
	public class GoogleLoginRequestDTO
	{
		public string Provider { get; set; } = string.Empty;

        public string IdToken { get; set; } = string.Empty;

        public string Role { get; set; } = string.Empty;

    }
}

