using System;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Eis.Identity.Api.Dtos;
using Microsoft.Extensions.Configuration;

namespace Eis.Identity.Api.SyncDataServices.Http
{
    public class HttpCommandDataClient : ICommandDataClient
    {
        private readonly HttpClient _httpClient;
        private readonly IConfiguration _config;

        public HttpCommandDataClient(HttpClient httpClient, IConfiguration config)
        {
            _httpClient = httpClient;
            _config = config;
        }

        public async Task SendIdentityToCommand(AppUserReadDto appUser)
        {
            var httpContent = new StringContent(
                JsonSerializer.Serialize(appUser),
                Encoding.UTF8,
                "application/json");

            var response = await _httpClient.PostAsync(_config["IdentityService"], httpContent);
            if (response.IsSuccessStatusCode)
            {
                Console.WriteLine("--> Sync POST to Identity Service was OK.");
            }
            else
            {
                Console.WriteLine("--> Sync POST to Identity Service failed.");
            }
        }
    }
}