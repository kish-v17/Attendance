using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Attendance.Models
{
    public class UserModel
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int UserId { get; set; }

        [Required(ErrorMessage = "Username is required.")]
        [StringLength(100, MinimumLength = 3, ErrorMessage = "Username must be between 3 and 100 characters.")]
        public string Fullname { get; set; }

        [Required(ErrorMessage = "Email is required.")]
        [EmailAddress(ErrorMessage = "Invalid email address.")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Password is required.")]
        [MinLength(8, ErrorMessage = "Password must be at least 8 characters long.")]
        public string Password { get; set; }

        [Required(ErrorMessage = "Role is required.")]
        [EnumDataType(typeof(UserRole), ErrorMessage = "Invalid role selected.")]
        public UserRole Role { get; set; }

        [Required]
        [DataType(DataType.DateTime)]
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }

    public enum UserRole
    {
        Admin =0,
        Faculty =1
    }
}
