using System.ComponentModel.DataAnnotations;

namespace TaskManager.Validations.Common;

public class WrapperResult(string errorMessage, ImportanceRating warningLevell) : ValidationResult(errorMessage)
{
    public ImportanceRating WarningLevel { get; set; } = warningLevell;
}
