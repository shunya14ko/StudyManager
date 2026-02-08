using System.ComponentModel.DataAnnotations;
using TaskManager.Validations.Common;

namespace TaskManager.Validations.Attributes;

public class ThresholdAttribute(int threshold, string errorMessage) : ValidationAttribute
{
    public int Threshold { get; } = threshold;
    public string UserErrorMessage { get; } = errorMessage;

    protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
    {
        // 常に「成功（Success）」を返して、標準バリデーションを無視
        var warningContext = validationContext.GetService(typeof(IWarningValidationContext));

        if (warningContext == null)
        {
            return ValidationResult.Success;
        }

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