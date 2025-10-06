using System;
namespace AuthenticationService.Core.DTOs
{
	public class GetProfileDTO
	{
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string? ImageUrl { get; set; }
        public string? Publicid { get; set; }
        public AddressDTO? Address { get; set; }
    }
}

