using System;
using System.Globalization;
using AuthenticationService.Core.Interfaces;
using AuthenticationService.Core.Utilities;
using AuthenticationService.Domain.Models;
using Microsoft.AspNetCore.Identity;

namespace AuthenticationService.Core.Services
{
	public class DigitTokenService : PhoneNumberTokenProvider<AppUser>, IDigitTokenService
	{
		public DigitTokenService()
		{

		}

		public const string DIGITPHONE = "DigitPhone";
        public const string DIGITEMAIL = "DigitEmail";

		public override Task<bool> CanGenerateTwoFactorTokenAsync(UserManager<AppUser> manager, AppUser user) => Task.FromResult(false);

		public override async Task<string> GenerateAsync(string purpose, UserManager<AppUser> manager, AppUser user)
		{
			var token = new SecurityToken(await manager.CreateSecurityTokenAsync(user));
			var modifier = await GetUserModifierAsync(purpose, manager, user);
			var code = Rfc6238AutnenticationProvider.GenerateCode(token, modifier).ToString("D6", CultureInfo.InvariantCulture);
			return code;
		}

		public override async Task<bool> ValidateAsync(string purpose, string token, UserManager<AppUser> manager, AppUser user)
		{
			if (!Int32.TryParse(token, out int code))
				return false;

			var securityToken = new SecurityToken(await manager.CreateSecurityTokenAsync(user));
			var modifier = await GetUserModifierAsync(purpose, manager, user);
			var valid = Rfc6238AutnenticationProvider.ValidateCode(securityToken, code, modifier, token.Length);
			return valid;
        }

		public override Task<string> GetUserModifierAsync(string purpose, UserManager<AppUser> manager, AppUser user)
		{
			return base.GetUserModifierAsync(purpose, manager, user);
		}
    }
}

