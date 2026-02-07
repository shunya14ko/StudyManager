using System.ComponentModel.DataAnnotations;
using TaskManager.Validations.Common;

namespace TaskManager.Validations.Attributes;

public class ThresholdAttribute(int threshold, string errorMessage) : ValidationAttribute
{
    [Required]
    public int Threshold { get; set; } = threshold;

    [Required]
    public string UserErrorMessage { get; set; } = errorMessage;

    protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
    {
        if (value is int intValue)
        {
            if (intValue < Threshold)
            {
                return new ExtendedValidationResult(UserErrorMessage, ImportanceRating.Warning);
            }
        }
        return ValidationResult.Success;
    }
}
