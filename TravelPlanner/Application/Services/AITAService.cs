using System.Net.Http.Headers;
using System.Text.Json;
using TravelPlanner.Application.DTOs;
using TravelPlanner.Core.Entities;
using TravelPlanner.Core.Interfaces.Repositories;
using TravelPlanner.Core.Interfaces.Services;

namespace TravelPlanner.Application.Services
{
    public class AITAService : IAITAService
    {
        private readonly HttpClient _httpClient;
        private readonly IAITARepository _aitaRepository;
        private readonly IAmadeusTokenGenerationService _amadeusTokenGenerationService;

        public AITAService(HttpClient httpClient, IAITARepository aitaRepository, IAmadeusTokenGenerationService amadeusTokenGeneration)
        {
            _httpClient = httpClient;
            _aitaRepository = aitaRepository;
            _amadeusTokenGenerationService = amadeusTokenGeneration;
        }

        public async Task<string> GetAITACodeAsync(AITARequest request)
        {
            var token = await _amadeusTokenGenerationService.GenerateAmadeusTokenAsync();
            
            var httpRequest = new HttpRequestMessage(HttpMethod.Get,
                $"https://test.api.amadeus.com/v1/reference-data/locations?subType=CITY&keyword={request.City}");

            httpRequest.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token.AccessToken);

            var response = await _httpClient.SendAsync(httpRequest);
            response.EnsureSuccessStatusCode();

            var responseBody = await response.Content.ReadAsStringAsync();

            var jsonDocument = JsonDocument.Parse(responseBody);
            string iataCode = jsonDocument.RootElement
                .GetProperty("data")[0]
                .GetProperty("iataCode")
                .GetString();

            await _aitaRepository.SaveCodeAsync(new AITA
            {
                City = request.City,
                AITACode = iataCode
            });

            return iataCode;
        }
    }
}
