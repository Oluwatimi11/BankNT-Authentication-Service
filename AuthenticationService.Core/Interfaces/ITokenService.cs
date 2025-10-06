using System;
using AuthenticationService.Domain.Models;

namespace AuthenticationService.Core.Interfaces
{
	public interface ITokenService
	{
		Task<string> GenerateToken(AppUser user);

		string GenerateRefreshToken();
	}
}

