using System;
using System.ComponentModel.DataAnnotations.Schema;
using AuthenticationService.Domain.Common;

namespace AuthenticationService.Domain.Models
{
	public class Address
	{
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]

        public string Id { get; set; } = Guid.NewGuid().ToString();

        public string Name { get; set; } = string.Empty;

		public string City { get; set; } = string.Empty;

		public string State { get; set; } = string.Empty;

        public DateTimeOffset CreatedAt { get; set; } = DateTimeOffset.Now;

        public DateTimeOffset UpdatedAt { get; set; } = DateTimeOffset.UtcNow;

        public string AppUserId { get; set; } = string.Empty;

		public AppUser? AppUser { get; set; } 
    }
}

