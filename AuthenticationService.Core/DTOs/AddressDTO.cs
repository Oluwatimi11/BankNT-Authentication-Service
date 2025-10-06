using System;
namespace AuthenticationService.Core.DTOs
{
	public class AddressDTO
	{
		public string Name { get; set; } = string.Empty;

		public string City { get; set; } = string.Empty;

        public string State { get; set; } = string.Empty;
    }
}

