namespace TransferProtocol.Exception;

public abstract class TransferBaseException : System.Exception
{
    public TransferBaseException(string message) : base(message) { }

    // 内部例外も渡せるようにコンストラクタを追加しておくのが一般的
    public TransferBaseException(string message, System.Exception? innerException)
        : base(message, innerException) { }
}