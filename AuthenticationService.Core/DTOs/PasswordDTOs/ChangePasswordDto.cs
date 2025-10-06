using System;
using System.ComponentModel.DataAnnotations;

namespace AuthenticationService.Core.DTOs.PasswordDTOs
{
	public class ChangePasswordDto
	{
		[Required, DataType(DataType.Password), Display(Name = "Current password")]
		public string CurrentPassword { get; set; } = string.Empty;

        [Required, DataType(DataType.Password), Display(Name = "New password")]
        public string NewPassword { get; set; } = string.Empty;

        [Required, DataType(DataType.Password), Display(Name = "Confirm new password")]
        [Compare("NewPassword", ErrorMessage = "Confirm new password does not match")]
        public string ConfirmNewPassword { get; set; } = string.Empty;
    }
}

