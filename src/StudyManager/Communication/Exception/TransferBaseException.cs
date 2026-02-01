namespace TransferProtocol.Exception;

// publicにして他プロジェクトから見えるようにする
// abstractなので、このクラス自体はインスタンス化できない（設計通り）
public abstract class TransferBaseException : System.Exception
{
    public TransferBaseException(string message) : base(message) { }

    // 内部例外も渡せるようにコンストラクタを追加しておくのが一般的
    public TransferBaseException(string message, System.Exception? innerException)
        : base(message, innerException) { }
}