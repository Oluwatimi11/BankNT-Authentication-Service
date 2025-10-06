using System;
namespace AuthenticationService.Core.DTOs
{
	public class CredentialResponseDTO
	{
		public string Id { get; set; } = string.Empty;

        public string Token { get; set; } = string.Empty;

        public string RefreshToken { get; set; } = string.Empty;

    }
}

