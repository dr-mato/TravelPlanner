using TravelPlanner.Core.Entities;
using TravelPlanner.Core.Interfaces;
using TravelPlanner.Application.DTOs;
using System.Text.Json;

namespace TravelPlanner.Infrastructure.Repositories;

public class OpenAIRepository : IRecommendationRepository
{
    private readonly HttpClient _httpClient;
    private readonly string _apiKey;

    public OpenAIRepository(HttpClient httpClient, IConfiguration config)
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
            """;

        var requestBody = new
        {
            model = "gpt-3.5-turbo",
            messages = new[] { new { role = "user", content = prompt } }
        };

        var response = await _httpClient.PostAsJsonAsync(
            "https://api.openai.com/v1/chat/completions",
            requestBody
        );

        var jsonResponse = await response.Content.ReadAsStringAsync();
        var aiResponse = JsonSerializer.Deserialize<OpenAIResponse>(jsonResponse);

        return JsonSerializer.Deserialize<List<Destination>>(
            aiResponse!.Choices[0].Message.Content
        ) ?? new List<Destination>();
    }

    private class OpenAIResponse
    {
        public List<Choice> Choices { get; set; } = new();
        public class Choice
        {
            public Message Message { get; set; } = new();
            public class Message
            {
                public string Content { get; set; } = string.Empty;
            }
        }
    }
}