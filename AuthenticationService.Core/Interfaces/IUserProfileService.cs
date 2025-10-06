using System;
using AuthenticationService.Core.DTOs;
using AuthenticationService.Domain.Models;
using Microsoft.AspNetCore.Http;

namespace AuthenticationService.Core.Interfaces
{
	public interface IUserProfileService
	{
        Task<ResponseDto<GetProfileDTO>> GetUserProfile(string id);

        Task<ResponseDto<GetUserDTO>> GetUserById(string id);

        Task<ResponseDto<string>> UploadImageAsync(string id, IFormFile file);

        Task<ResponseDto<string>> RemoveImageAsync(string id);
	}
}

