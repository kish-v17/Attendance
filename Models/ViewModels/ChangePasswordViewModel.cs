using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;

namespace Attendance.Models.ViewModels
{
    public class ChangePasswordViewModel
    {
        [ValidateNever]
        [Required(ErrorMessage = "Current Password is required")]
        public string CurrentPassword { get; set; }
        
        [Required(ErrorMessage = "New Password is required")]
        [MinLength(6)]
        public string NewPassword { get; set; }

        [Required (ErrorMessage = "Confirm Password is required")]
        [Compare("NewPassword", ErrorMessage = "Passwords do not match.")]
        public string ConfirmPassword { get; set; }
    }

}
