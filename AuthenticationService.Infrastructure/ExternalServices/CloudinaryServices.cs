using System;
using AuthenticationService.Core.DTOs;
using AuthenticationService.Core.Interfaces;
using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using ILogger = Serilog.ILogger;

namespace AuthenticationService.Infrastructure.ExternalServices
{
	public class CloudinaryServices : ICloudinaryServices
    {
        private ILogger _logger;
        private readonly IConfiguration _configuration;
        private readonly Cloudinary _cloudinary;

		public CloudinaryServices(IServiceProvider provider, IConfiguration configuration, ILogger logger)
		{
            _logger = logger;
            _configuration = configuration;
            _cloudinary = new Cloudinary(new Account(_configuration.GetValue<string>("CloudinarySettings:CloudName"),
                _configuration.GetValue<string>("CloudinarySettings:ApiKey"),
                _configuration.GetValue<string>("CloudinarySettings:ApiSecret")));
		}

        private bool ValidateImage(IFormFile image)
        {
            // validate the image size and extension type using settings from appsettings
            var status = false;
            string[] listOfextensions = { ".jpg", ".jpeg", ".png" };
            if (image == null) return status;
            for (int i = 0; i < listOfextensions.Length; i++)
            {
                if (image.FileName.EndsWith(listOfextensions[i]))
                {
                    status = true;
                    break;
                }
            }
            if (status)
            {
                var pixSize = Convert.ToInt64(_configuration.GetValue<string>("PhotoSettings:Size"));
                if (image.Length > pixSize)
                    return !status;
            }
            return status;
        }

        public async Task<UploadResult> UpdateByPublicId(IFormFile file, string publicId)
        {
            var response = new ResponseDto<bool>();
            var uploadResult = new ImageUploadResult();
            await using var imageStream = file.OpenReadStream();
            var parameters = new ImageUploadParams()
            {
                File = new FileDescription(publicId, imageStream),
                PublicId = publicId,
                Overwrite = true,
                UniqueFilename = true
            };
            _logger.Information("Finish Image Uplaod Setup, about loading to cloudinary ");
            uploadResult = await _cloudinary.UploadAsync(parameters);
            return uploadResult;
        }

        /// <summary>
        /// Uploads a single image
        /// </summary>
        /// <param name="file"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task<UploadResult> UploadImage(IFormFile file)
        {
            _logger.Information("Enter the upload image service");
            //Runtime complexity check needed.
            var result = ValidateImage(file);
            _logger.Information($"Check result {result}");
            var uploadResult = new ImageUploadResult();
            if (!result)
            {
                return default;
            }
            var fileName = Guid.NewGuid().ToString() + "_" + file.FileName;
            using (var imageStream = file.OpenReadStream())
            {
                _logger.Information($"Check the imageStream {imageStream}");
                var parameters = new ImageUploadParams()
                {
                    File = new FileDescription(fileName, imageStream),
                    PublicId = fileName
                };
                _logger.Information("Finish Image Upload Setup, about loading to Cloudinary...");
                uploadResult = await _cloudinary.UploadAsync(parameters);
            };
            _logger.Information("Upload to Cloudinary Successful");
            return uploadResult;
        }

        public async Task<bool> DeleteByPublicId(string publicId)
        {
            try
            {
                var deleteParams = new DeletionParams(publicId);
                await _cloudinary.DestroyAsync(deleteParams);
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
    }
}

