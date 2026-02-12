namespace TaskManager.LokiData;

public class LogModel
{
    // Lokiが付与するタイムスタンプ用
    public DateTime Timestamp { get; set; }

    // 以下、PythonスクリプトのJSONの中身
    public string Level { get; set; } = "";
    public string Endpoint { get; set; } = "";
    public int Status { get; set; }
    public string Message { get; set; } = "";
    public int DurationMs { get; set; }
}