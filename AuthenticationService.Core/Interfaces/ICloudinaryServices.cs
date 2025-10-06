using System;
using CloudinaryDotNet.Actions;
using Microsoft.AspNetCore.Http;

namespace AuthenticationService.Core.Interfaces
{
	public interface ICloudinaryServices
	{
		Task<UploadResult> UpdateByPublicId(IFormFile file, string publicId);

		Task<UploadResult> UploadImage(IFormFile file);

		Task<bool> DeleteByPublicId(string publicId);
	}
}

