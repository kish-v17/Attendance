using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;

namespace Attendance.Models.ViewModels
{
    public class LoginViewModel
    {
        // LOGIN + FORGET PASSWORD
        [Required(ErrorMessage = "Email is required.")]
        [EmailAddress(ErrorMessage = "Invalid email address.")]
        [ValidateNever]
        public string Email { get; set; }

        // LOGIN
        [Required(ErrorMessage = "Password is required.")]
        [DataType(DataType.Password)]
        [ValidateNever]
        public string? Password { get; set; }

        // OTP VERIFICATION
        [Display(Name = "OTP")]
        [ValidateNever]
        public string? OTP { get; set; }

        // RESET PASSWORD
        [DataType(DataType.Password)]
        [Display(Name = "New Password")]
        [ValidateNever]
        public string? NewPassword { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirm Password")]
        [Compare("NewPassword", ErrorMessage = "Passwords do not match.")]
        [ValidateNever]
        public string? ConfirmPassword { get; set; }
    }
}
