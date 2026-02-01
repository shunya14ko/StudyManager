using System.Net.Http.Json;
using Communication.Services.Models;
using TransferProtocol.Exception;

namespace Communication.Services;

public class HttpClientRest(HttpClient client)
{
    private readonly HttpClient _httpClient = client;
    private static readonly string projectMainUri = "http://localhost:5327/api/v1/mainProject";

    public async Task UpdateMainProjectAsync(MainProjectDto model)
    {
        try
        {
            var response = await _httpClient.PutAsJsonAsync(projectMainUri, model);
            response.EnsureSuccessStatusCode();
        }
        catch(Exception ex)
        {
            throw new TransferRestException(ex.Message);
        }
    }
}