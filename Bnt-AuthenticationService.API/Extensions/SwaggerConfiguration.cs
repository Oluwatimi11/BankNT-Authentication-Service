using System;
using System.Reflection;
using Microsoft.OpenApi.Models;
using AuthenticationService.Core.Services;

namespace Bnt_AuthenticationService.API.Extensions
{
	public static class SwaggerConfigurationExtension
	{
        public static void AddSwaggerConfiguration(this IServiceCollection services)
        {
            services.AddSwaggerGen(setupAction =>
            {
                setupAction.SwaggerDoc("FintechOpenAPI", new()
                {
                    Title = "Bank Note Tracker API",
                    Version = "1",
                    Description = "A tracking web API with authentification and notification features"
                });
                // Set the comments path for the Swagger JSON and UI.
                var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                setupAction.IncludeXmlComments(xmlPath);

                // To Enable authorization using Swagger (JWT) 
                setupAction.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme()
                {
                    Name = "Authorization",
                    Type = SecuritySchemeType.ApiKey,
                    Scheme = "Bearer",
                    BearerFormat = "JWT",
                    In = ParameterLocation.Header,
                    Description = "Enter 'Bearer' [space] and then your valid token in the text input below.\r\n\r\nExample: \"Bearer eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9\"",
                });
                setupAction.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "Bearer"
                            }
                        },
                        Array.Empty<string>()
                    }
                });
                setupAction.OperationFilter<FileUploadOperation>();
            });
        }
    }
}

