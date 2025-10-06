using System;
namespace AuthenticationService.Domain.Common
{
	public interface IAuditable
	{

        public DateTimeOffset CreatedAt { get; set; }

        public DateTimeOffset UpdatedAt { get; set; }
    }
}

