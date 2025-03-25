using TravelPlanner.Core.Entities;
using TravelPlanner.Application.DTOs;
using System.Text.Json;
using OpenAI.Chat;
using TravelPlanner.Core.Interfaces.Services;

namespace TravelPlanner.Application.Services;

public class OpenAIService : IRecommendationService
{
    private readonly HttpClient _httpClient;
    private readonly string _apiKey;

    public OpenAIService(HttpClient httpClient, IConfiguration config)
    {
        _httpClient = httpClient;
        _apiKey = config["OpenAI:ApiKey"]!;
        _httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {_apiKey}");
    }

    public async Task<List<Destination>> GetRecommendationsAsync(UserPreferences preferences)
    {
        var prompt = $"""
            Act as a travel expert. Recommend 3 destinations for:
            - Budget: {preferences.Budget}
            - Interests: {string.Join(", ", preferences.Interests)}
            - Travel Style: {preferences.TravelStyle}
            - Weather: {preferences.WeatherPreference ?? "any"}

            Format the response as JSON with:
            name, country, summary, budgetEstimate, weatherMatch.
            """
        ;

        ChatClient chatClient = new(model: "gpt-4", apiKey: _apiKey);
        ChatCompletion completion = await chatClient.CompleteChatAsync(prompt);

        string response = completion.Content[0].Text;

        return JsonSerializer.Deserialize<List<Destination>>(response) ?? new List<Destination>();
    }
}
