using System;
namespace AuthenticationService.Core.DTOs.RefreshTokenDTOs
{
	public class RefreshTokenResponseDTO
	{
		public string NewAccessToken { get; set; } = null!;

		public string NewRefreshToken { get; set; } = null!;

    }
}

