using System.ComponentModel.DataAnnotations;

namespace TaskManager.Validations.Common;

public class ExtendedValidationResult(string errorMessage, ImportanceRating importance) : ValidationResult(errorMessage)
{
    public ImportanceRating Importance { get; } = importance;
}