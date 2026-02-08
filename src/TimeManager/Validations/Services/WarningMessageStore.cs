using Microsoft.AspNetCore.Components.Forms;

namespace TaskManager.Validations.Services;

public class WarningMessageStore
{
    // Key: ViewModelのプロパテ
    // Value: 警告メッセージのリスト
    private readonly Dictionary<FieldIdentifier, List<string>> _messages = [];

    public event Action? OnWarningStateChanged;

    private void NotifyStateChanged() => OnWarningStateChanged?.Invoke();

    public void Add(FieldIdentifier fieldIdentifier, string message)
    {
        // TryGetValue で値の取得
        if (!_messages.TryGetValue(fieldIdentifier, out var list))
        {
            // 存在しない場合のみ新しく作成して登録
            list = [];
            _messages[fieldIdentifier] = list;
        }

        // 取得（または作成）したリストに追加
        list.Add(message);
        NotifyStateChanged();
    }

    /// <summary>
    /// 指定したフィールドの警告をクリア
    /// 入力が変わったタイミングなどで呼び出す
    /// </summary>
    public void Clear(FieldIdentifier fieldIdentifier)
    {
        if (_messages.Remove(fieldIdentifier))
        {
            NotifyStateChanged();
        }
    }

    /// <summary>
    /// フォーム送信時の再検証前などに呼び出す
    /// </summary>
    public void ClearAll()
    {
        if (_messages.Count != 0)
        {
            _messages.Clear();
            NotifyStateChanged();
        }
    }

    /// <summary>
    /// 指定したフィールドの警告メッセージを取得
    /// </summary>
    public IEnumerable<string> Get(FieldIdentifier fieldIdentifier)
    {
        return _messages.TryGetValue(fieldIdentifier, out var messages) ? messages : Enumerable.Empty<string>();
    }

    /// <summary>
    /// ダイアログの有無などの警告感知に使用
    /// </summary>
    public bool HasWarnings() => _messages.Count != 0;
}