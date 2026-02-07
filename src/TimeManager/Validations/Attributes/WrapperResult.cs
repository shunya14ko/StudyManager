using System.ComponentModel.DataAnnotations;
using TaskManager.Validations.Abstract;

namespace TaskManager.Validations.Attributes;

public class WrapperResult(string errorMessage, ImportanceRating warningLevell) : ValidationResult(errorMessage)
{
    public ImportanceRating WarningLevel { get; set; } = warningLevell;
}
