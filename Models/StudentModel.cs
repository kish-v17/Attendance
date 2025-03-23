using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Attendance.Data;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.EntityFrameworkCore;

namespace Attendance.Models
{
    [Index(nameof(EnrollmentNumber), IsUnique = true)]
    public class StudentModel
    {
        [Key]
        public int StudentId { get; set; }

        [Required(ErrorMessage = "Full Name is required.")]
        [StringLength(100, ErrorMessage = "Full Name cannot exceed 100 characters.")]
        public string FullName { get; set; }

        [Required(ErrorMessage = "Enrollment Number is required.")]
        [StringLength(50)]
        public string EnrollmentNumber { get; set; }

        [Required(ErrorMessage = "Father's Name is required.")]
        [StringLength(50)]
        public string FatherName { get; set; }

        [Required(ErrorMessage = "Mother's Name is required.")]
        [StringLength(50)]
        public string MotherName { get; set; }

        [Required(ErrorMessage = "Gender is required.")]
        public Gender Gender { get; set; }

        [Required(ErrorMessage = "Date of Birth is required.")]
        [DataType(DataType.Date)]
        public DateOnly DateOfBirth { get; set; }

        [Required(ErrorMessage = "Aadhar Card Number is required.")]
        [StringLength(12, ErrorMessage = "Aadhar Card Number must be 12 digits.")]
        public string AadharCardNumber { get; set; }

        [Required(ErrorMessage = "Blood Group is required.")]
        public BloodGroup BloodGroup { get; set; }

        [Required(ErrorMessage = "Email is required.")]
        [EmailAddress(ErrorMessage = "Invalid Email format.")]
        [StringLength(100)]
        public string Email { get; set; }

        [Required(ErrorMessage = "Mobile Number is required.")]
        [StringLength(15, ErrorMessage = "Invalid Mobile Number.")]
        public string MobileNo { get; set; }

        [Required(ErrorMessage = "Parent Mobile Number is required.")]
        [StringLength(15, ErrorMessage = "Invalid Mobile Number.")]
        public string ParentMobileNo { get; set; }

        [Required(ErrorMessage = "Category is required.")]
        public Category Category { get; set; }

        [Required(ErrorMessage = "Class is required.")]
        [ForeignKey("Class")]
        public int ClassId { get; set; }

        public ClassModel Class { get; set; }
        

        [Required(ErrorMessage = "Address is required.")]
        [StringLength(255)]
        public string Address { get; set; }

        [Required(ErrorMessage = "City is required.")]
        [StringLength(50)]
        public string City { get; set; }

        [Required(ErrorMessage = "State is required.")]
        [StringLength(50)]
        public string State { get; set; }

        [Required(ErrorMessage = "Country is required.")]
        [StringLength(50)]
        public string Country { get; set; }

        [Required(ErrorMessage = "Pin Code is required.")]
        [StringLength(6)]
        public string PinCode { get; set; }

        [ValidateNever]
        public List<ClassModel> ClassModels { get; set; }

        //public void GenerateEnrollmentNumber(AppDBContext context)
        //{
        //    var studentClass = context.ClassTbl
        //        .Include(c => c.Batch) // Include Batch to access Year
        //        .ThenInclude(b => b.Course) // Include Course to access CourseName
        //        .FirstOrDefault(c => c.ClassId == this.ClassId);

        //    if (studentClass == null || studentClass.Batch == null || studentClass.Batch.Course == null)
        //        throw new Exception("Class, Batch, or Course not found!");

        //    string year = studentClass.Batch.Year.ToString(); // Fetch year from BatchModel
        //    string courseCode = studentClass.BatchTbl.Course.CourseName.Substring(0, 3).ToUpper(); // First 3 letters of CourseName
        //    int count = context.StudentTbl.Count(s => s.ClassId == this.ClassId) + 1; // Get student count in this class

        //    this.EnrollmentNumber = $"{year}-{courseCode}-{count:D5}"; // Example: 2024-CSE-00001
        //}

        public static ValidationResult ValidateAgeLimit(DateTime dateOfBirth, ValidationContext context)
        {
            int age = DateTime.Today.Year - dateOfBirth.Year;
            if (dateOfBirth > DateTime.Today.AddYears(-age)) age--;

            if (age < 17 || age > 30)
                return new ValidationResult("Student age must be between 17 and 30 years.");

            return ValidationResult.Success;
        }
    }

}

public enum Gender
{
    Male =1,
    Female=2,
    Other=3
}

public enum BloodGroup
{
    A_Positive=1,
    A_Negative=2,
    B_Positive=3,
    B_Negative=4,
    AB_Positive=5,
    AB_Negative=6,
    O_Positive=7,
    O_Negative=8
}

public enum Category
{
    OPEN=1,
    OBC=2,
    SC=3,
    ST=4
}