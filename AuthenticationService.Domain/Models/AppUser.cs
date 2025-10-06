using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using AuthenticationService.Domain.Common;
using Microsoft.AspNetCore.Identity;

namespace AuthenticationService.Domain.Models
{
	public class AppUser : IdentityUser, IAuditable
    {
        public string FirstName { get; set; } = string.Empty;

        public string LastName { get; set; } = string.Empty;

        public bool IsActive { get; set; }

        [StringLength(11)]
        public string BVN { get; set; } = string.Empty;

        public string RefreshToken { get; set; } = string.Empty;

        public DateTime RefreshTokenExpiryTime { get; set; }

        public DateTimeOffset CreatedAt { get; set; } = DateTimeOffset.Now;

        public DateTimeOffset UpdatedAt { get; set; } = DateTimeOffset.UtcNow;

        public ICollection<Address> Addresses { get; set; }

        public string? ImageUrl { get; set; }

        public string? Publicid { get; set; }

        public AppUser() => Addresses = new HashSet<Address>();

    }
}

