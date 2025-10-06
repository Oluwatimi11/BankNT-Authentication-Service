using System;
using System.IdentityModel.Tokens.Jwt;
using System.Net.Http;
using System.Security.Claims;
using System.Text;
using AuthenticationService.Core.Interfaces;
using AuthenticationService.Domain.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace AuthenticationService.Core.Services
{
	public class TokenService : ITokenService
	{

        private readonly IConfiguration _configuration;
        private readonly UserManager<AppUser> _manager;

		public TokenService(IConfiguration configuration, UserManager<AppUser> manager)
		{
            _manager = manager;
            _configuration = configuration;
        }

        public async Task<string> GenerateToken(AppUser user)
        {
            var authClaims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id),
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(ClaimTypes.GivenName, user.FirstName),
                new Claim(ClaimTypes.Surname, user.LastName),
            };
            var roles = await _manager.GetRolesAsync(user);
            foreach (var role in roles)
            {
                authClaims.Add(new Claim(ClaimTypes.Role, role));
            }
            var signingKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration.GetValue<string>("Jwt:Token")));
            var jwtConfig = _configuration.GetSection("Jwt");
            var token = new JwtSecurityToken
                (
                issuer: _configuration.GetValue<string>("Jwt:Issuer"),
                claims: authClaims,
                expires: DateTime.Now.AddMinutes(Convert.ToDouble(jwtConfig.GetSection("lifetime").Value)),
                signingCredentials: new SigningCredentials(signingKey, SecurityAlgorithms.HmacSha256));

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        public string GenerateRefreshToken()
        {
            return Guid.NewGuid().ToString();
        }


    }
}

