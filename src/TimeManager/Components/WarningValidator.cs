using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using System.ComponentModel.DataAnnotations;
using System.Reflection;
using TaskManager.Validations.Common;
using TaskManager.Validations.Services;

namespace TaskManager.Components;

public class WarningValidator : ComponentBase, IDisposable
{
    [CascadingParameter]
    private EditContext CurrentEditContext { get; set; } = default!;

    [Inject]
    private WarningMessageStore WarningStore { get; set; } = default!;

    // 変更前のコンテキストを覚えておく変数
    private EditContext? _previousEditContext;

    protected override void OnParametersSet()
    {
        // 以前のDataと今回のDataが異なる場合の処理
        if (CurrentEditContext != _previousEditContext)
        {
            if (_previousEditContext != null)
            {
                _previousEditContext.OnFieldChanged -= HandleFieldChanged;
                _previousEditContext.OnValidationRequested -= HandleValidationRequested;
            }

            if (CurrentEditContext != null)
            {
                CurrentEditContext.OnFieldChanged += HandleFieldChanged;
                CurrentEditContext.OnValidationRequested += HandleValidationRequested;
                ValidateAll();
            }

            _previousEditContext = CurrentEditContext;
        }
    }

    // 初期レンダリング時のバリデータ
    internal void ValidateAll()
    {
        WarningStore.ClearAll();

        var model = CurrentEditContext.Model;
        var properties = model.GetType().GetProperties();

        // 全プロパティを走査して検証
        foreach (var property in properties)
        {
            ValidateProperty(model, property);
        }
    }

    private void HandleFieldChanged(object? sender, FieldChangedEventArgs eventArgs)
    {
        // 変更されたフィールドの識別子を取得
        var fieldIdentifier = eventArgs.FieldIdentifier;

        // そのフィールドの古い警告をクリア
        WarningStore.Clear(fieldIdentifier);

        // リフレクションでプロパティ情報を取得して検証
        // Note: FieldIdentifier.Model は、ネストされたオブジェクトの場合、その子オブジェクトを指します
        var property = fieldIdentifier.Model.GetType().GetProperty(fieldIdentifier.FieldName);
        if (property != null)
        {
            ValidateProperty(fieldIdentifier.Model, property);
        }
    }

    private void HandleValidationRequested(object? sender, ValidationRequestedEventArgs eventArgs)
    {
        // 全体の検証、既存の警告を全クリア
        WarningStore.ClearAll();

        var model = CurrentEditContext.Model;
        var properties = model.GetType().GetProperties();

        // 全プロパティを走査して検証
        foreach (var property in properties)
        {
            ValidateProperty(model, property);
        }
    }

    private void ValidateProperty(object model, PropertyInfo property)
    {
        var value = property.GetValue(model);

        // ここで WarningServiceProvider を生成して渡しているため、WarningValidator クラス自体が IServiceProvider を実装する必要はない
        var serviceProvider = new WarningServiceProvider();

        var context = new ValidationContext(model, serviceProvider, items: null) { MemberName = property.Name };
        var results = new List<ValidationResult>();

        // ServiceProviderを渡しているので、ThresholdAttribute が反応して結果を返す
        Validator.TryValidateProperty(value, context, results);

        foreach (var result in results)
        {
            if (result is ExtendedValidationResult extResult && extResult.Importance == ImportanceRating.Warning)
            {
                var fieldIdentifier = new FieldIdentifier(model, property.Name);
                WarningStore.Add(fieldIdentifier, result.ErrorMessage!);
            }
        }
    }

    public void Dispose()
    {
        if (CurrentEditContext != null)
        {
            CurrentEditContext.OnFieldChanged -= HandleFieldChanged;
            CurrentEditContext.OnValidationRequested -= HandleValidationRequested;
        }
        WarningStore.ClearAll();
        GC.SuppressFinalize(this);
    }
}