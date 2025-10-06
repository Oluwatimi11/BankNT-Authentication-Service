using System;
namespace AuthenticationService.Core.DTOs.RefreshTokenDTOs
{
	public class RefreshTokenRequestDTO
	{
		public string UserId { get; set; } = string.Empty;

		public string RefreshToken { get; set; } = string.Empty;
	}
}

