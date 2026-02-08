using System.ComponentModel.DataAnnotations;
using TaskManager.Validations.Common;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;

namespace TaskManager.Validations.Services;

public class EditContextPipeline : ComponentBase
{
    // 現在の EditContext を取得
    [CascadingParameter]
    private EditContext CurrentEditContext { get; set; } = default!;
    private ValidationMessageStore _messageStore = default!;

    protected override void OnInitialized()
    {
        if (CurrentEditContext == null) { return; }

        _messageStore = new ValidationMessageStore(CurrentEditContext);

        // フィールド変更時に警告を再計算するイベントを購読
        CurrentEditContext.OnFieldChanged += (s, e) => HandleValidation(e.FieldIdentifier);
    }
     
    private void HandleValidation(FieldIdentifier fieldIdentifier)
    {
        _messageStore.Clear(fieldIdentifier);

        var propertyInfo = fieldIdentifier.Model.GetType().GetProperty(fieldIdentifier.FieldName);
        var value = propertyInfo?.GetValue(fieldIdentifier.Model);
        var validationContext = new ValidationContext(fieldIdentifier.Model) { MemberName = fieldIdentifier.FieldName };
        var results = new List<ValidationResult>();

        Validator.TryValidateProperty(value, validationContext, results);

        foreach (var result in results)
        {
            if (result is ExtendedValidationResult ext && ext.WarningLevel == ImportanceRating.Warning)
            {
                _messageStore.Add(fieldIdentifier, result.ErrorMessage!);
            }
        }

        CurrentEditContext.NotifyValidationStateChanged();
    }

    public bool HasWarnings(object model)
    {
        var results = new List<ValidationResult>();
        Validator.TryValidateObject(model, new ValidationContext(model), results, true);

        return results.OfType<ExtendedValidationResult>().Any(r => r.WarningLevel == ImportanceRating.Warning);
    }

    public bool HasFatalErrors(object model)
    {
        var results = new List<ValidationResult>();
        Validator.TryValidateObject(model, new ValidationContext(model), results, true);

        return results.OfType<ExtendedValidationResult>().Any(r => r.WarningLevel != ImportanceRating.Warning);
    }
}
