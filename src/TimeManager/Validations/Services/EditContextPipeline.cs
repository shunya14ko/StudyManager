using System.ComponentModel.DataAnnotations;
using TaskManager.Validations.Common;

namespace TaskManager.Validations.Services;

// Validations/Services/EditContextPipeline.cs
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using TaskManager.Validations.Common;

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
        CurrentEditContext.OnFieldChanged += (s, e) => ValidateWarning(e.FieldIdentifier);
        CurrentEditContext.OnValidationRequested += (s, e) => ValidateAllWarnings();
    }

    private void ValidateWarning(FieldIdentifier fieldIdentifier)
    {
        _messageStore.Clear(fieldIdentifier);

        // リフレクション等でモデルから属性を取得し、警告をチェック
        // 警告がある場合のみ _messageStore.Add(fieldIdentifier, "警告メッセージ") を実行

        // ポイント：ここで ValidationResult は返さないため、
        // editContext.Validate() 自体の戻り値（True/False）には影響を与えません。
    }

    private void ValidateAllWarnings()
    {
        _messageStore.Clear();
        // モデル全体の警告をスキャン
    }

    public bool HasWarnings(object model)
    {
        var context = new ValidationContext(model);
        var results = new List<ValidationResult>();

        // 全プロパティのバリデーションを実行
        Validator.TryValidateObject(model, context, results, true);

        // ここで「Warningレベルのものがあるか」を判定
        // ※Attribute側で「警告だけど ValidationResult を返す」ように一時的に変えて判定するロジックなど
        return results.OfType<ExtendedValidationResult>().Any(r => r.WarningLevel == ImportanceRating.Warning);
    }


    /// <summary>
    /// ここの使い方については要検討
    /// </summary>
    private void OnDispose()
    {
        _messageStore.Clear(); 
    }

    private void Dispose()
    {
        OnDispose();
    }
}
