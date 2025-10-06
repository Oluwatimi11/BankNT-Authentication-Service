using System;
using System.Net;
using AuthenticationService.Core.AppSettings;
using AuthenticationService.Core.DTOs;
using AuthenticationService.Core.DTOs.AuthDTOs;
using AuthenticationService.Core.DTOs.EmailDTOs;
using AuthenticationService.Core.DTOs.PasswordDTOs;
using AuthenticationService.Core.DTOs.RefreshTokenDTOs;
using AuthenticationService.Core.Interfaces;
using AuthenticationService.Core.Utilities;
using AuthenticationService.Domain.Enums;
using AuthenticationService.Domain.Models;
using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace AuthenticationService.Core.Services
{
	public class AuthService : IAuthService
	{
        public readonly ITokenService _tokenService;
        public readonly IDigitTokenService _digitTokenService;
        public readonly IHttpClientService _httpClientService;
        private readonly ILogger _logger;
        public readonly UserManager<AppUser> _userManager;
        private readonly GoogleSettings _googleSettings;
        private readonly NotificationSettings _notificationSettings;
        private readonly PaymentSettings _paymentSettings;
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;

		public AuthService(IServiceProvider provider, IMapper mapper, IUnitOfWork unitOfWork)
		{
            _userManager = provider.GetRequiredService<UserManager<AppUser>>();
            _tokenService = provider.GetRequiredService<ITokenService>();
            _digitTokenService = provider.GetRequiredService<IDigitTokenService>();
            _httpClientService = provider.GetRequiredService<IHttpClientService>();
            _logger = provider.GetRequiredService<ILogger>();
            _googleSettings = provider.GetRequiredService<GoogleSettings>();
            _notificationSettings = provider.GetRequiredService<NotificationSettings>();
            _paymentSettings = provider.GetRequiredService<PaymentSettings>();
            _mapper = mapper;
            _unitOfWork = unitOfWork;
        }

        public async Task<ResponseDto<string>> ChangePassword(ChangePasswordDto model, string userId)
        {
            ResponseDto<string> response = null;
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                response = ResponseDto<string>.Fail("User not found.");
            }
            var isPasswordConfirmed = await _userManager.CheckPasswordAsync(user, model.CurrentPassword);
            if (!isPasswordConfirmed)
            {
                response = ResponseDto<string>.Fail("Current Password is incorrect.");
            }
            var result = await _userManager.ChangePasswordAsync(user, model.CurrentPassword, model.NewPassword);
            if (result.Succeeded)
            {
                response = ResponseDto<string>.Success("Successful!", "Password  has been Updated");
            }
            return response;
        }



        public async Task<ResponseDto<string>> ConfirmEmail(ConfirmEmailDTO confirmEmailDTO)
        {
            var user = await _userManager.FindByEmailAsync(confirmEmailDTO.EmailAddress);
            if (user == null)
            {
                return ResponseDto<string>.Fail("User not found", (int)HttpStatusCode.NotFound);
            }
            var purpose = UserManager<AppUser>.ConfirmEmailTokenPurpose;
            var result = await _digitTokenService.ValidateAsync(purpose, confirmEmailDTO.Token, _userManager, user);
            if (result)
            {
                user.EmailConfirmed = true;
                var update = await _userManager.UpdateAsync(user);
                if (update.Succeeded)
                {
                    return ResponseDto<string>.Success("Email Confirmation successful", user.Id, (int)HttpStatusCode.OK);
                }
            }
            return ResponseDto<String>.Success("Email Confirmation successful", user.Id, (int)HttpStatusCode.InternalServerError);
        }



        public async Task<ResponseDto<string>> ForgotPassword(ForgotPasswordDTO model)
        {
            var user = await _userManager.FindByEmailAsync(model.EmailAddress);
            if (user is null)
                return ResponseDto<string>.Fail("This email does not exist on this app", (int)HttpStatusCode.NotFound);
            var purpose = UserManager<AppUser>.ResetPasswordTokenPurpose;
            var token = await _digitTokenService.GenerateAsync(purpose, _userManager, user);
            var mailBody = await EmailBodyBuilder.GetEmailBody(user, "StaticFiles/HTML/ForgotPassword.html", token);
            var emailNotification = new EmailNotificationDTO
            {
                ToEmail = user.Email,
                Subject = "Reset Password",
                Message = mailBody,
            };
            var notificationService = await _httpClientService.PostRequestAsync<EmailNotificationDTO,
                ResponseDto<bool>>(_notificationSettings.BaseUrl, "send-email", emailNotification);
            if (!notificationService.Status)
                return ResponseDto<string>.Fail("An error occur, try again later.", (int)HttpStatusCode.InternalServerError);
            return ResponseDto<string>.Success($"This email is successfully to: {model.EmailAddress}",
                $"A reset link was successfully sent to {model.EmailAddress}", (int)HttpStatusCode.OK);

        }



        public async Task<ResponseDto<CredentialResponseDTO>> Login(LoginDTO model)
        {
            var user = await _userManager.FindByEmailAsync(model.Email);
            if (user == null)
            {
                return ResponseDto<CredentialResponseDTO>.Fail("User does not exist", (int)HttpStatusCode.NotFound);
            }

            if (!await _userManager.CheckPasswordAsync(user, model.Password))
            {
                return ResponseDto<CredentialResponseDTO>.Fail("Invalid user credentials", (int)HttpStatusCode.BadRequest);
            }

            if (!user.EmailConfirmed)
            {
                return ResponseDto<CredentialResponseDTO>.Fail("User's account is not confirmed", (int)HttpStatusCode.BadRequest);
            }
            else if (!user.IsActive)
            {
                return ResponseDto<CredentialResponseDTO>.Fail("User's account is deactivated", (int)HttpStatusCode.BadRequest);
            }

            user.RefreshToken = _tokenService.GenerateRefreshToken();
            user.RefreshTokenExpiryTime = DateTime.Now.AddDays(7);
            var credentialResponse = new CredentialResponseDTO()
            {
                Id = user.Id,
                Token = await _tokenService.GenerateToken(user),
                RefreshToken = user.RefreshToken
            };

            var result = await _userManager.UpdateAsync(user);
            if (result.Succeeded)
            {
                _logger.LogInformation("User successfully logged in");
                return ResponseDto<CredentialResponseDTO>.Success("Login successful", credentialResponse);
            }
            return ResponseDto<CredentialResponseDTO>.Fail("Failed to login user", (int)HttpStatusCode.InternalServerError);
        }



        public async Task<ResponseDto<RefreshTokenResponseDTO>> RefreshTokenAsync(RefreshTokenRequestDTO refreshToken)
        {
            var response = new ResponseDto<RefreshTokenRequestDTO>();
            var tokenToBeRefreshed = refreshToken.RefreshToken;
            var userId = refreshToken.UserId;
            var user = await _userManager.FindByIdAsync(userId);
            int value = DateTime.Compare((DateTime)user?.RefreshTokenExpiryTime!, DateTime.Now);
            if(user.RefreshToken != tokenToBeRefreshed || value < 0)
            {
                return ResponseDto<RefreshTokenResponseDTO>.Fail("Invalid credentials", (int)HttpStatusCode.BadRequest);
            }
            var refreshMapping = new RefreshTokenResponseDTO
            {
                NewAccessToken = await _tokenService.GenerateToken(user),
                NewRefreshToken = _tokenService.GenerateRefreshToken()
            };

            user.RefreshToken = refreshMapping.NewRefreshToken;
            user.RefreshTokenExpiryTime = DateTime.Now.AddDays(7);
            await _userManager.UpdateAsync(user);

            return ResponseDto<RefreshTokenResponseDTO>.Success("Token refreshed successfully", refreshMapping);
        }


        private async Task<bool> SendEmail(AppUser userModel)
        {
            string token = await _digitTokenService.GenerateAsync("Email Verification", _userManager, userModel);
            var mailBody = await EmailBodyBuilder.GetEmailBody(userModel, "StaticFiles/HTML/EmailConfirmation.html", token);
            var sendEmail = new EmailNotificationDTO
            {
                ToEmail = userModel.Email,
                Subject = "Email Verification",
                Message = mailBody
            };

            var mailService = await _httpClientService.PostRequestAsync<EmailNotificationDTO, ResponseDto<bool>>(_notificationSettings.BaseUrl, "send-email", sendEmail);
            return mailService.Status;
        }


        private async Task<bool> CreateWallet(AppUser userModel, string pin)
        {
            var getUser = await _userManager.FindByEmailAsync(userModel.Email);
            var walletModel = new CreateWalletDTO
            {
                FirstName = getUser.FirstName,
                LastName = getUser.LastName,
                UserEmail = getUser.Email,
                Pin = pin,
                UserId = getUser.Id
            };
            var createWallet = await _httpClientService.PostRequestAsync<CreateWalletDTO, ResponseDto<bool>>(_paymentSettings.BaseUrl, "api/Wallets/createwallet", walletModel);
            if (!createWallet.Status)
                return false;
            return await SendEmail(userModel);
        }


        public async Task<ResponseDto<string>> Register(RegistrationDTO userDetails)
        {
            var response = new ResponseDto<string>();

            var _validator = new UserValidator();
            await _validator.ValidateAsync(userDetails);

            var checkEmail = await _userManager.FindByEmailAsync(userDetails.Email);
            if(checkEmail != null)
            {
                return ResponseDto<string>.Fail("Email already Exists", (int)HttpStatusCode.BadRequest);
            }
            var userModel = _mapper.Map<AppUser>(userDetails);
            var result = await _userManager.CreateAsync(userModel, userDetails.Password);
            var res = await _userManager.AddToRoleAsync(userModel, UserRole.Customer.ToString());
            var IsWalletCreated = await CreateWallet(userModel, userDetails.Pin);
            if (!IsWalletCreated)
                return ResponseDto<string>.Fail("Internal Server Error", (int)HttpStatusCode.BadRequest);
            return ResponseDto<string>.Success("Registration Successful", "User Successfully Added", (int)HttpStatusCode.Created);
        }



        public async Task<ResponseDto<string>> ResendOTP(ResendOtpDTO model)
        {
            var user = await _userManager.FindByEmailAsync(model.Email);
            if (user == null)
            {
                return ResponseDto<string>.Fail("Email does not exist", (int)HttpStatusCode.NotFound);
            }

            var purpose = (model.Purpose == "ConfirmEmail") ? UserManager<AppUser>.ConfirmEmailTokenPurpose
                : UserManager<AppUser>.ResetPasswordTokenPurpose;
            var token = await _digitTokenService.GenerateAsync(purpose, _userManager, user);

            var mailBody = await EmailBodyBuilder.GetEmailBody(user, emailTempPath: (model.Purpose == "ConfirmEmail") ?
                "StaticFiles/EmailConfirmation.html" : "StaticFiles/ForgotPassword.html", token);

            var emailNotification = new EmailNotificationDTO
            {
                ToEmail = user.Email,
                Subject = "Email Verification",
                Message = mailBody,
            };

            var notificationService = await _httpClientService.PostRequestAsync<EmailNotificationDTO,
                ResponseDto<bool>>(_notificationSettings.BaseUrl, "send-email", emailNotification);

            if (notificationService.Data)
                if (!user.IsActive)
                {
                    return ResponseDto<string>.Success($"This email is successfully to: {model.Email}",
                        $"OTP was successfully reset to {model.Email}");
                }
            return ResponseDto<string>.Fail("Sending OTP was not successful", (int)HttpStatusCode.InternalServerError);
        }


        public async Task<ResponseDto<string>> ResetPasswordAsync(ResetPasswordDTO resetPasswordDTO)
        {
            var validator = new ResetPasswordValidator();
            await validator.ValidateAsync(resetPasswordDTO);
            _logger.LogInformation("Reset password attempt");
            var user = await _userManager.FindByEmailAsync(resetPasswordDTO.Email);
            if (user == null)
            {
                return ResponseDto<string>.Fail("Email does not exist", (int)HttpStatusCode.NotFound);
            }
            var purpose = UserManager<AppUser>.ResetPasswordTokenPurpose;
            var isValidToken = await _digitTokenService.ValidateAsync(purpose, resetPasswordDTO.Token, _userManager, user);
            var result = new IdentityResult();
            var hasher = new PasswordHasher<AppUser>();
            if (isValidToken)
            {
                var hash = hasher.HashPassword(user, resetPasswordDTO.NewPassword);
                user.PasswordHash = hash;
                result = await _userManager.UpdateAsync(user);
            }
            if (result.Succeeded)
            {
                return ResponseDto<string>.Success("Password has been reset successfully", user.Id, (int)HttpStatusCode.OK);
            }
            return ResponseDto<string>.Fail("Invalid Token", (int)HttpStatusCode.BadRequest);
        }

        public Task<ResponseDto<CredentialResponseDTO>> VerifyGoogleToken(GoogleLoginRequestDTO google)
        {
            throw new NotImplementedException();
        }
    }
}

