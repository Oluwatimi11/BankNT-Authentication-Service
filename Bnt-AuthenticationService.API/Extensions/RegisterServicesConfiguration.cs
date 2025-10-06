using System;
using AuthenticationService.Core.AppSettings;
using AuthenticationService.Core.DTOs.AuthDTOs;
using AuthenticationService.Core.DTOs.PasswordDTOs;
using AuthenticationService.Core.Interfaces;
using AuthenticationService.Core.Services;
using AuthenticationService.Core.Utilities;
using AuthenticationService.Infrastructure.ExternalServices;
using AuthenticationService.Infrastructure.Repository;
using FluentValidation;
using Microsoft.Extensions.Options;
using Serilog;

namespace Bnt_AuthenticationService.API.Extensions
{
	public static class RegisterServicesConfiguration
	{
        public static void AddRegisterServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<GoogleSettings>(configuration.GetSection(nameof(GoogleSettings)));
            services.Configure<NotificationSettings>(configuration.GetSection(nameof(NotificationSettings)));
            services.Configure<PaymentSettings>(configuration.GetSection(nameof(PaymentSettings)));
            services.Configure<UserSettings>(configuration.GetSection(nameof(UserSettings)));

            services.AddCors(o =>
            {
                o.AddPolicy("AllowAll", builder =>
                    builder.AllowAnyOrigin()
                    .AllowAnyMethod()
                    .AllowAnyHeader()
                    );
            });

            services.AddSingleton(Log.Logger);
            services.AddScoped(cfg => cfg.GetRequiredService<IOptions<GoogleSettings>>().Value);
            services.AddScoped(cfg => cfg.GetRequiredService<IOptions<NotificationSettings>>().Value);
            services.AddScoped(cfg => cfg.GetRequiredService<IOptions<PaymentSettings>>().Value);
            services.AddScoped(cfg => cfg.GetRequiredService<IOptions<UserSettings>>().Value);

            services.AddScoped<ICloudinaryServices, CloudinaryServices>();
            services.AddScoped<ITokenService, TokenService>();
            services.AddScoped<IDigitTokenService, DigitTokenService>();
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddScoped<IAuthService, AuthService>();
            services.AddScoped<IUserProfileService, UserProfileService>();
            services.AddTransient<IValidator<ResetPasswordDTO>, ResetPasswordValidator>();


            services.AddTransient<IValidator<ForgotPasswordDTO>, EmailValidator>();
            services.AddTransient<IValidator<LoginDTO>, LoginUserValidator>();
            services.AddTransient<IValidator<RegistrationDTO>, UserValidator>();
            services.AddHttpClient<IHttpClientService, HttpClientService>();
            services.AddScoped<IHttpClientService, HttpClientService>();
        }
    }
}

