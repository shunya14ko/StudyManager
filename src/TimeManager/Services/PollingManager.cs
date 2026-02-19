using System.Text.Json;
using System.Text.Json.Serialization;
using TaskManager.LokiData;

namespace TaskManager.Services;

public class PollingManager(HttpClient httpClient) : IPollingManager
{
    private readonly HttpClient _httpClient = httpClient;

    public async Task StartPolling(IProgress<LogsModel> progress, CancellationToken cts)
    {
        var query = "{container=\"loki-log-generator-1\"}";
        var url = $"http://localhost:3100/loki/api/v1/query_range?query={Uri.EscapeDataString(query)}&limit=100";

        async Task FetchAndReportAsync()
        {
            try
            {
                var response = await _httpClient.GetFromJsonAsync<LokiResponse>(url, cts);
                if (response?.Data?.Result != null)
                {
                    foreach (var stream in response.Data.Result)
                    {
                        foreach (var value in stream.Values)
                        {
                            var logString = value[1];
                            try
                            {
                                var logModel = JsonSerializer.Deserialize<LogsModel>(logString, new JsonSerializerOptions
                                {
                                    // string case ignore
                                    PropertyNameCaseInsensitive = true 
                                });

                                if (logModel is not null)
                                {
                                    // progress
                                    progress.Report(logModel); 
                                }
                            }
                            catch (JsonException)
                            {
                                // Ignore
                            }
                        }
                    }
                }
            }
            catch (OperationCanceledException)
            {
                // UI progress
                throw;
            }
            catch (Exception)
            {
                // Logger
            }
        }
        await FetchAndReportAsync();
        using PeriodicTimer timer = new(TimeSpan.FromSeconds(10));
        while (await timer.WaitForNextTickAsync(cts))
        {
            await FetchAndReportAsync();
        }
    }

    public class LokiResponse
    {
        [JsonPropertyName("status")]
        public string Status { get; set; } = string.Empty;
        [JsonPropertyName("data")]
        public LokiData Data { get; set; } = new();
    }

    public class LokiData
    {
        [JsonPropertyName("resultType")]
        public string ResultType { get; set; } = string.Empty;
        [JsonPropertyName("result")]
        public List<LokiResult> Result { get; set; } = [];
    }

    public class LokiResult
    {
        [JsonPropertyName("stream")]
        public Dictionary<string, string> Stream { get; set; } = [];
        [JsonPropertyName("values")]
        public List<List<string>> Values { get; set; } = [];
    }
}