using TaskManager.LokiData;

namespace TaskManager.Services;

public class PollingManager(HttpClient httpClient) : IPollingManager
{
    private readonly HttpClient _httpClient = httpClient;

    public async Task StartPolling(IProgress<LogsModel> progress, CancellationToken cts)
    {

        var query = "{container=\"loki-log-generator-1\"}";
        var url = $"http://localhost:3100/loki/api/v1/query_range?query={Uri.EscapeDataString(query)}&limit=100";

        // Initial fetch
        var response = await _httpClient.GetFromJsonAsync<LogsModel>(url, cts);

        if (response is not null)
        {
            progress.Report(response);
        }

        using PeriodicTimer timer = new(TimeSpan.FromSeconds(10));
        while (await timer.WaitForNextTickAsync(cts))
        {
            var res = await _httpClient.GetFromJsonAsync<LogsModel>(url, cts);
            if (res is not null)
            {
                progress.Report(res);
            }
        }
    }
}
