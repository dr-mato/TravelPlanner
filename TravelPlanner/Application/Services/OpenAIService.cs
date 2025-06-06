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

    public async Task<List<DailyPlan>> GetDailyPlanAsync(DailyPlanRequest request)
    {
        var prompt = $@"
        You are an expert travel planner. Based on the following information, create a detailed daily plan for the user:

        - Destination: {request.Destination.Name}, {request.Destination.Country}
        - Budget Estimate: {request.Destination.BudgetEstimate}
        - Hotel: {request.Hotel.Name}, priced at {request.Hotel.Price}{request.Hotel.Currency}
        - Flights: {string.Join(", ", request.Flights.Select(f => $"{f.FlightNumber} costing {f.Price}"))}
        - Daily Weather Forecast: {string.Join(", ", request.Weathers.Select(w => $"{w.Date.ToShortDateString()} - {w.Temperature}°C"))}

        Notes:
        - The user's total budget includes the cost of the flight and hotel, so you should consider how much money remains for daily activities.
        - Plan one set of activities per day, considering the weather conditions.
        - Be creative but realistic with the activities and advice.
        - Assume the user is staying at the hotel provided.

        Format your response as a JSON array, where each element represents a day's plan with the following structure:
        {{
            ""DayNumber"": int,      // Day number starting from 1
            ""MorningActivity"": string,  // Activity planned for the morning
            ""AfternoonActivity"": string, // Activity planned for the afternoon
            ""EveningActivity"": string,   // Activity planned for the evening
            ""Notes"": string             // Any important notes, tips, or advice
        }}

        Example response:
        [
            {{
            ""DayNumber"": 1,
            ""MorningActivity"": ""Visit the city museum"",
            ""AfternoonActivity"": ""Boat tour along the river"",
            ""EveningActivity"": ""Dinner at a recommended seafood restaurant"",
            ""Notes"": ""Expect sunny weather, wear comfortable shoes.""
            }},
            {{
            ""DayNumber"": 2,
            ""MorningActivity"": ""Hiking trip to nearby hills"",
            ""AfternoonActivity"": ""Relax at the hotel spa"",
            ""EveningActivity"": ""Explore the local night market"",
            ""Notes"": ""Possibility of light rain in the evening, bring an umbrella.""
            }}
        ]
        ";


        ChatClient chatClient = new(model: "gpt-4", apiKey: _apiKey);
           ChatCompletion completion = await chatClient.CompleteChatAsync(prompt);
    
           string response = completion.Content[0].Text;
           Console.WriteLine(response ?? "No response data");
    
           var options = new JsonSerializerOptions
           {
            PropertyNameCaseInsensitive = true
           };
    
           var dailyPlan = JsonSerializer.Deserialize<List<DailyPlan>>(response, options) ?? new List<DailyPlan>();
    
           return dailyPlan;
    }

    public async Task<List<DailyPlan>> GetDailyPlanTwoAsync(DailyPlanTwoRequest request)
    {
        var prompt = $@"
        You are an expert travel planner. Based on the following information, create a detailed daily plan for the user:

        - Destination: {request.Destination.Name}, {request.Destination.Country}
        - Budget Estimate: {request.Destination.BudgetEstimate}
        - Hotel: {request.Hotel.Name}, priced at {request.Hotel.Price}{request.Hotel.Currency}
        - Flights: {string.Join(", ", request.Flights.Select(f => $"Flight costing {f.Price}"))}
        - Daily Weather Forecast: {string.Join(", ", request.Weathers.Select(w => $"{w.Date.ToShortDateString()} - {w.Temperature}°C"))}

        Notes:
        - The user's total budget includes the cost of the flight and hotel, so you should consider how much money remains for daily activities.
        - Plan one set of activities per day, considering the weather conditions.
        - Be creative but realistic with the activities and advice.
        - Assume the user is staying at the hotel provided.

        Format your response as a JSON array, where each element represents a day's plan with the following structure:
        {{
            ""DayNumber"": int,      // Day number starting from 1
            ""MorningActivity"": string,  // Activity planned for the morning
            ""AfternoonActivity"": string, // Activity planned for the afternoon
            ""EveningActivity"": string,   // Activity planned for the evening
            ""Notes"": string             // Any important notes, tips, or advice
        }}

        Example response:
        [
            {{
            ""DayNumber"": 1,
            ""MorningActivity"": ""Visit the city museum"",
            ""AfternoonActivity"": ""Boat tour along the river"",
            ""EveningActivity"": ""Dinner at a recommended seafood restaurant"",
            ""Notes"": ""Expect sunny weather, wear comfortable shoes.""
            }},
            {{
            ""DayNumber"": 2,
            ""MorningActivity"": ""Hiking trip to nearby hills"",
            ""AfternoonActivity"": ""Relax at the hotel spa"",
            ""EveningActivity"": ""Explore the local night market"",
            ""Notes"": ""Possibility of light rain in the evening, bring an umbrella.""
            }}
        ]
        ";


        ChatClient chatClient = new(model: "gpt-4", apiKey: _apiKey);
        ChatCompletion completion = await chatClient.CompleteChatAsync(prompt);

        string response = completion.Content[0].Text;
        Console.WriteLine(response ?? "No response data");

        var options = new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        };

        var dailyPlan = JsonSerializer.Deserialize<List<DailyPlan>>(response, options) ?? new List<DailyPlan>();

        return dailyPlan;
    }

    public async Task<List<DailyPlan>> GetDailyPlanThreeAsync(DailyPlanThreeRequest request)
    {
        var prompt = $@"
        You are an expert travel planner. Based on the following information, create a detailed daily plan for the user:

        - Destination: {request.Destination.Name}, {request.Destination.Country}
        - Budget Estimate: {request.Destination.BudgetEstimate}
        - Hotel: {request.Hotel.HotelName}, priced at {request.Hotel.Price}
        - Flights: {string.Join(", ", request.Flights.Select(f => $"Flight costing {f.Price}"))}
        - Daily Weather Forecast: {string.Join(", ", request.Weathers.Select(w => $"{w.Date.ToShortDateString()} - {w.Temperature}°C"))}

        Notes:
        - The user's total budget includes the cost of the flight and hotel, so you should consider how much money remains for daily activities.
        - Plan one set of activities per day, considering the weather conditions.
        - Be creative but realistic with the activities and advice.
        - Assume the user is staying at the hotel provided.

        Format your response as a JSON array, where each element represents a day's plan with the following structure:
        {{
            ""DayNumber"": int,      // Day number starting from 1
            ""MorningActivity"": string,  // Activity planned for the morning
            ""AfternoonActivity"": string, // Activity planned for the afternoon
            ""EveningActivity"": string,   // Activity planned for the evening
            ""Notes"": string             // Any important notes, tips, or advice
        }}

        Example response:
        [
            {{
            ""DayNumber"": 1,
            ""MorningActivity"": ""Visit the city museum"",
            ""AfternoonActivity"": ""Boat tour along the river"",
            ""EveningActivity"": ""Dinner at a recommended seafood restaurant"",
            ""Notes"": ""Expect sunny weather, wear comfortable shoes.""
            }},
            {{
            ""DayNumber"": 2,
            ""MorningActivity"": ""Hiking trip to nearby hills"",
            ""AfternoonActivity"": ""Relax at the hotel spa"",
            ""EveningActivity"": ""Explore the local night market"",
            ""Notes"": ""Possibility of light rain in the evening, bring an umbrella.""
            }}
        ]
        ";


        ChatClient chatClient = new(model: "gpt-4", apiKey: _apiKey);
        ChatCompletion completion = await chatClient.CompleteChatAsync(prompt);

        string response = completion.Content[0].Text;
        Console.WriteLine(response ?? "No response data");

        var options = new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        };

        var dailyPlan = JsonSerializer.Deserialize<List<DailyPlan>>(response, options) ?? new List<DailyPlan>();

        return dailyPlan;
    }
}

