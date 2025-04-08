using TravelPlanner.Core.Entities;
using TravelPlanner.Application.DTOs;
using System.Text.Json;
using OpenAI.Chat;
using TravelPlanner.Core.Interfaces.Services;
using TravelPlanner.Core.Interfaces.Repositories;

namespace TravelPlanner.Application.Services;

public class OpenAIService : IOpenAIService
{
    private readonly HttpClient _httpClient;
    private readonly string _apiKey;
    private readonly IDestinationRepository _destinationRepository;

    public OpenAIService(HttpClient httpClient, IConfiguration config, IDestinationRepository destinationRepository)
    {
        _httpClient = httpClient;
        _apiKey = config["OpenAI:ApiKey"]!;
        _httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {_apiKey}");
        _destinationRepository = destinationRepository;
    }

    public async Task<List<Destination>> GetRecommendationsAsync(UserPreferences preferences)
    {
        var prompt = $@"
        Act as a travel expert. Recommend 3 destinations for:
        - Budget: {preferences.Budget}
        - Interests: {string.Join(",", preferences.Interests)}
        - Travel Style: {preferences.TravelStyle}
        - Weather: {preferences.WeatherPreference ?? "any"}

        Format the response as a JSON array with each object containing:
        - Name (string)
        - Country (string)
        - Summary (string)
        - BudgetEstimate (number, without quotes)
        - WeatherMatch (boolean, true or false)

        Example response:
        {{
          ""Name"": ""Bali, Indonesia"",
          ""Country"": ""Indonesia"",
          ""Summary"": ""A tropical paradise..."",
          ""BudgetEstimate"": 5000,
          ""WeatherMatch"": true
        }}
        ";

        ChatClient chatClient = new(model: "gpt-4", apiKey: _apiKey);
        ChatCompletion completion = await chatClient.CompleteChatAsync(prompt);

        string response = completion.Content[0].Text;
        Console.WriteLine(response ?? "No response data");

        var options = new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        };

        var destinations = JsonSerializer.Deserialize<List<Destination>>(response, options) ?? new List<Destination>();

        foreach(var destination in destinations)
        {
            await _destinationRepository.AddAsync(destination);
            await _destinationRepository.SaveChangesAsync();
        }

        return destinations;
    }
}
