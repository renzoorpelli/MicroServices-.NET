using System.Net.Http;
using System.Text;
using System.Text.Json;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using PlatformService.DTOs.Platform;

namespace PlatformService.Services.Http.SyncDataServices;

public class HttpCommandDataClients : ICommandDataClient
{
    private readonly IHttpClientFactory _httpClientFactory;
    private readonly ILogger<HttpCommandDataClients> _logger;


    public HttpCommandDataClients(IHttpClientFactory httpClientFactory,
        ILogger<HttpCommandDataClients> logger)
    {
        _httpClientFactory = httpClientFactory;
        _logger = logger;
    }
    public async Task<bool> SendPlatformToCommand(PlatformReadDto platform)
    {
        var httpContent = new StringContent(JsonConvert.SerializeObject(platform), 
            Encoding.UTF8, 
            "application/json");

        var httpClient = _httpClientFactory.CreateClient("CommandService");
        var response = await httpClient.PostAsync("platform", httpContent);
        if (!response.IsSuccessStatusCode)
        {
            _logger.LogInformation($"--> Respuesta servidor {response.StatusCode}, surgio un problema.");
            return false;
        }
        _logger.LogInformation("--> Respuesta servidor 200 (OK), se envió la plataforma correctamente");
        return true;
    }
}