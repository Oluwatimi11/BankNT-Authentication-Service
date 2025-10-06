using System;
using AuthenticationService.Core.DTOs;
using AuthenticationService.Core.DTOs.AuthDTOs;
using AuthenticationService.Core.DTOs.EmailDTOs;
using AuthenticationService.Core.DTOs.PasswordDTOs;
using AuthenticationService.Core.DTOs.RefreshTokenDTOs;

namespace AuthenticationService.Core.Interfaces
{
	public interface IAuthService
	{
		Task<ResponseDto<string>> ChangePassword(ChangePasswordDto model, string userId);

        Task<ResponseDto<CredentialResponseDTO>> VerifyGoogleToken(GoogleLoginRequestDTO google);

        Task<ResponseDto<CredentialResponseDTO>> Login(LoginDTO model);

        Task<ResponseDto<RefreshTokenResponseDTO>> RefreshTokenAsync(RefreshTokenRequestDTO refreshToken);

        Task<ResponseDto<string>> ResendOTP(ResendOtpDTO model);

        Task<ResponseDto<string>> ResetPasswordAsync(ResetPasswordDTO resetPasswordDTO);

        Task<ResponseDto<string>> Register(RegistrationDTO model);

        Task<ResponseDto<string>> ConfirmEmail(ConfirmEmailDTO confirmEmailDTO);

        Task<ResponseDto<string>> ForgotPassword(ForgotPasswordDTO model);

    }
}

