using System.ComponentModel.DataAnnotations;
using TaskManager.Validations.Abstract;

namespace TaskManager.Validations.Attributes;

public class ThresholdAttribute(int threshold, string errorMessage) : ValidationAttribute
{
    [Required]
    public int Threshold { get; set; } = threshold;

    // これ宣言側で上手く制御できるかもしれない？　ただ、Nullチェックが入るのであった方がいいかも？？
    [Required]
    public string UserErrorMessage { get; set; } = errorMessage;

    protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
    {
        if (value is int intValue)
        {
            if (intValue < Threshold)
            {
                return new WrapperResult(UserErrorMessage, ImportanceRating.warning);
            }
        }
        return ValidationResult.Success;
    }
}
