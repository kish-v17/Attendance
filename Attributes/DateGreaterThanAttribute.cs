using System.ComponentModel.DataAnnotations;

namespace Attendance.Attributes
{
    public class DateGreaterThanAttribute : ValidationAttribute
    {
        private readonly string _comparisonProperty;

        public DateGreaterThanAttribute(string comparisonProperty)
        {
            _comparisonProperty = comparisonProperty;
        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var comparisonProperty = validationContext.ObjectType.GetProperty(_comparisonProperty);
            if (comparisonProperty == null)
                return new ValidationResult($"Unknown property: {_comparisonProperty}");

            var comparisonValue = comparisonProperty.GetValue(validationContext.ObjectInstance);
            var currentValue = value;

            if (comparisonValue is DateOnly startDate && currentValue is DateOnly endDate)
            {
                if (endDate <= startDate)
                {
                    return new ValidationResult($"{validationContext.DisplayName} must be after {_comparisonProperty}.");
                }
            }

            return ValidationResult.Success;
        }
    }
}
