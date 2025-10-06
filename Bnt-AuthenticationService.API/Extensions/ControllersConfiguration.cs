using System;
using FluentValidation.AspNetCore;
using AuthenticationService.Core.DTOs;
using AuthenticationService.Core.Utilities;

namespace Bnt_AuthenticationService.API.Extensions
{
	public static class ControllersConfiguration
	{
        public static void AddControllersExtension(this IServiceCollection services)
        {
            services.AddControllers()
            .AddFluentValidation(fv =>
            {
                fv.DisableDataAnnotationsValidation = true;
                fv.RegisterValidatorsFromAssemblyContaining<Program>();
                fv.ImplicitlyValidateChildProperties = true;
            })
            .AddNewtonsoftJson(op => op.SerializerSettings.ReferenceLoopHandling
               = Newtonsoft.Json.ReferenceLoopHandling.Ignore);
        }
    }
}

