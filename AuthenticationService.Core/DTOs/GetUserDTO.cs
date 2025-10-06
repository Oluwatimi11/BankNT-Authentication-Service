using System;
namespace AuthenticationService.Core.DTOs
{
	public class GetUserDTO
	{
        public string Id { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
    }
}

