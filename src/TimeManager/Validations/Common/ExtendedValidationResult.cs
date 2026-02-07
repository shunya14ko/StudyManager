using System.ComponentModel.DataAnnotations;

namespace TaskManager.Validations.Common;

public class ExtendedValidationResult(string errorMessage, ImportanceRating warningLevell) : ValidationResult(errorMessage)
{
    public ImportanceRating WarningLevel { get; set; } = warningLevell;
}
