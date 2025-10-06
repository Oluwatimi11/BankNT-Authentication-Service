using System;
using Microsoft.AspNetCore.Http;

namespace AuthenticationService.Core.DTOs
{
	public class UploadImageDTO
	{
		public IFormFile? ImageToUpload { get; set; }
	}
}

