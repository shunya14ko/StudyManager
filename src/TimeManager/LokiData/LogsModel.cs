namespace TaskManager.LokiData;

public class LogsModel
{
    public DateTime Timestamp { get; set; }
    public string Level { get; set; } = string.Empty;
    public string Endpoint { get; set; } = string.Empty;
    public int Status { get; set; }
    public string Message { get; set; } = string.Empty;
    public int DurationMs { get; set; }
}