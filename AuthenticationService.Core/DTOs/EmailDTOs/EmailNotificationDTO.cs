using System;
namespace AuthenticationService.Core.DTOs.EmailDTOs
{
	public class EmailNotificationDTO
	{
		public string ToEmail { get; set; } = string.Empty;

        public string Subject { get; set; } = string.Empty;

        public string Message { get; set; } = string.Empty;

    }
}

