namespace TransferProtocol.Exception;

public class TransferRestException : TransferBaseException
{
    public TransferRestException() : base("通信エラーが発生しました。") { }

    public TransferRestException(string message) : base(message) { }

    public TransferRestException(string message, System.Exception innerException)
        : base(message, innerException) { }
}