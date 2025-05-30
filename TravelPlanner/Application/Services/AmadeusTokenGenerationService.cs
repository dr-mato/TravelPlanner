using System.Text.Json;
using TravelPlanner.Application.DTOs;
using TravelPlanner.Core.Entities;
using TravelPlanner.Core.Interfaces.Repositories;
using TravelPlanner.Core.Interfaces.Services;

namespace TravelPlanner.Application.Services
{
    public class AmadeusTokenGenerationService : IAmadeusTokenGenerationService
    {
        private readonly HttpClient _httpClient;
        private readonly string _apiKey;
        private readonly string _apiSecret;
        private readonly IAmadeusTokenRepository _tokenRepository;

        public AmadeusTokenGenerationService(HttpClient httpClient, IConfiguration config, IAmadeusTokenRepository tokenRepository)
        {
            _httpClient = httpClient;
            _apiKey = config["Amadeus:ApiKey"]!;
            _apiSecret = config["Amadeus:ApiSecret"]!;
            _tokenRepository = tokenRepository;
        }

        public async Task<AmadeusToken> GenerateAmadeusTokenAsync()
        {
            var savedToken = await _tokenRepository.GetCurrentTokenAsync();

            if (savedToken != null && savedToken.ExpirationTime > DateTime.UtcNow)
            {
                return savedToken;
            }
            else
            {
                var request = new HttpRequestMessage(HttpMethod.Post, "https://test.api.amadeus.com/v1/security/oauth2/token")
                {
                    Content = new FormUrlEncodedContent(new[]
    {
                    new KeyValuePair<string, string>("grant_type", "client_credentials"),
                    new KeyValuePair<string, string>("client_id", _apiKey),
                    new KeyValuePair<string, string>("client_secret", _apiSecret),
                })
                };

                var response = await _httpClient.SendAsync(request);
                response.EnsureSuccessStatusCode();

                var responseContent = await response.Content.ReadAsStringAsync();
                Console.WriteLine(responseContent);

                var tokenResponse = JsonSerializer.Deserialize<AmadeusTokenResponse>(responseContent, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });

                if (savedToken== null)
                {
                    var token = new AmadeusToken
                    {
                        AccessToken = tokenResponse.Access_Token,
                        ExpirationTime = DateTime.UtcNow.AddSeconds(tokenResponse.Expires_In)
                    };
                    await _tokenRepository.SaveTokenAsync(token);
                    return token;
                }
                else
                {
                    savedToken.AccessToken = tokenResponse.Access_Token;
                    savedToken.ExpirationTime = DateTime.UtcNow.AddSeconds(tokenResponse.Expires_In);
                    _tokenRepository.Update(savedToken);
                    return savedToken;
                }
            }
        }
    }
}
