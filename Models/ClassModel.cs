using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Attendance.Models
{
    public class ClassModel
    {
        [Key]
        public int ClassId { get; set; }

        [Required(ErrorMessage = "Batch ID is required.")]
        [Range(1, int.MaxValue, ErrorMessage = "Invalid Batch ID.")]
        public int BatchId { get; set; }

        [ForeignKey("BatchId")]
        public BatchModel Batch { get; set; }

        [Required(ErrorMessage = "Class letter is required.")]
        [RegularExpression("^[A-Z]$", ErrorMessage = "Class must be a single uppercase letter (A-Z).")]
        public string Class { get; set; }

        [ValidateNever]
        public List<BatchModel>? Batches { get; set; }
    }
}