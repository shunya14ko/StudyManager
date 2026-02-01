using System.Net.Http.Json;
using Communication.Services.Models;
using TransferProtocol.Exception;

namespace Communication.Services;

public class HttpClientRest(HttpClient client)
{
    private readonly HttpClient _httpClient = client;
    private static readonly string projectMainUri = "http://localhost:5215/mainProject";

    public async Task UpdateMainProjectAsync(MainProjectDto model)
    {
        try
        {
            var response = await _httpClient.PutAsJsonAsync(projectMainUri, model);
            response.EnsureSuccessStatusCode();
        }
        catch
        {
            throw new TransferRestException();
        }
    }
}