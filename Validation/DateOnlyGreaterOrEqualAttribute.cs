using System.ComponentModel.DataAnnotations;
using System.Reflection;

namespace StajyerTakip.Validation
{
    /// <summary>
    /// DateOnly karşılaştırması: bu alan >= diğer alan
    /// Örn: [DateOnlyGreaterOrEqual(nameof(StartDate))]
    /// </summary>
    public class DateOnlyGreaterOrEqualAttribute : ValidationAttribute
    {
        private readonly string _otherProperty;

        public DateOnlyGreaterOrEqualAttribute(string otherProperty)
        {
            _otherProperty = otherProperty;
        }

        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            var otherProp = validationContext.ObjectType.GetProperty(_otherProperty, BindingFlags.Public | BindingFlags.Instance);
            if (otherProp == null) return ValidationResult.Success;

            var otherVal = otherProp.GetValue(validationContext.ObjectInstance);

            // Hem DateOnly hem DateOnly? destekle
            DateOnly? current = value is DateOnly d1 ? d1 : value as DateOnly?;
            DateOnly? other   = otherVal is DateOnly d2 ? d2 : otherVal as DateOnly?;

            if (current.HasValue && other.HasValue && current.Value < other.Value)
            {
                return new ValidationResult(ErrorMessage ?? "Geçersiz tarih karşılaştırması.");
            }

            return ValidationResult.Success;
        }
    }
}
