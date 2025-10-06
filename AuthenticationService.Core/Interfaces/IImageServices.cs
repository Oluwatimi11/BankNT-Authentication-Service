using System;
using AuthenticationService.Core.DTOs;
using Microsoft.AspNetCore.Http;

namespace AuthenticationService.Core.Interfaces
{
	public interface IImageServices
	{
		Task<ResponseDto<string>> UploadImageAsync(string Id, IFormFile file);
	}
}

