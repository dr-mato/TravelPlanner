using System.Text.Json;
using TravelPlanner.Application.DTOs;
using TravelPlanner.Core.Entities;
using TravelPlanner.Core.Interfaces.Repositories;
using TravelPlanner.Core.Interfaces.Services;

namespace TravelPlanner.Application.Services
{
    public class WeatherService : IWeatherService
    {
        private readonly HttpClient _httpClient;
        private readonly string _apiKey;
        private readonly IWeatherRepository _weatherRepository;

        public WeatherService(HttpClient httpClient, IConfiguration config, IWeatherRepository weatherRepository)
        {
            _httpClient = httpClient;
            _apiKey = config["VisualCrossing:ApiKey"]!;
            _weatherRepository = weatherRepository;
        }

        public async Task<List<Weather>> GetWeatherRangeForLocationAsync(WeatherRequest request)
        {
            bool needsHistorical = request.EndDate > DateTime.Now.AddDays(15);
            string endpoint;

            if (await _weatherRepository.HasAllDataForRangeAsync(request.Location, request.StartDate, request.EndDate))
            {
                return await _weatherRepository.GetExistingWeatherAsync(request.Location, request.StartDate, request.EndDate);
            }

            if (needsHistorical)
            {
                DateTime historicalStartDate = request.StartDate.AddYears(-1);
                DateTime historicalEndDate = request.EndDate.AddYears(-1);

                string formattedStartDate = historicalStartDate.ToString("yyyy-MM-dd");
                string formattedEndDate = historicalEndDate.ToString("yyyy-MM-dd");
                endpoint = $"https://weather.visualcrossing.com/VisualCrossingWebServices/rest/services/timeline/" +
                    $"{Uri.EscapeDataString(request.Location)}/{formattedStartDate}/{formattedEndDate}";
            }
            else
            {
                endpoint = $"https://weather.visualcrossing.com/VisualCrossingWebServices/rest/services/timeline/" +
                    $"{Uri.EscapeDataString(request.Location)}";
            }

            return await FetchWeatherRangeDataAsync(endpoint, request.StartDate, request.EndDate, request.Location);
        }

        private async Task<List<Weather>> FetchWeatherRangeDataAsync(string endpoint, DateTime startDate, DateTime endDate, string location)
        {
            var requestUri = $"{endpoint}?unitGroup=metric&key={_apiKey}&contentType=json";

            var response = await _httpClient.GetAsync(requestUri);
            response.EnsureSuccessStatusCode();

            var content = await response.Content.ReadAsStringAsync();
            var jsonDocument = JsonDocument.Parse(content);

            var result = new Dictionary<DateTime, double>();

            if (jsonDocument.RootElement.TryGetProperty("days", out var daysElement) &&
                daysElement.ValueKind == JsonValueKind.Array)
            {
                foreach (var day in daysElement.EnumerateArray())
                {
                    if (day.TryGetProperty("datetime", out var dateElement) &&
                        day.TryGetProperty("temp", out var tempElement))
                    {
                        if (DateTime.TryParse(dateElement.GetString(), out var date) &&
                            date.Date >= startDate.Date && date.Date <= endDate.Date)
                        {
                            result[date.Date] = tempElement.GetDouble();
                        }
                    }
                }
            }

            var weatherList = new List<Weather>();
            foreach (var kvp in result)
            {
                var weather = new Weather
                {
                    Location = location,
                    Date = kvp.Key,
                    Temperature = kvp.Value
                };
                weatherList.Add(weather);
            }
            await _weatherRepository.AddWeatherDataAsync(weatherList);

            return weatherList;
        }
    }
}
