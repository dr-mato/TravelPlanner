using TravelPlanner.Core.Entities;
using TravelPlanner.Core.Interfaces.Repositories;
using TravelPlanner.Core.Interfaces.Services;
using static System.Net.WebRequestMethods;

namespace TravelPlanner.Application.Services
{
    public class FlightService: IFlightService
    {
        private readonly HttpClient _httpClient;
        private readonly string _apiKey;
        private readonly IFlightRepository _flightRepository;

        public FlightService(HttpClient httpClient, IConfiguration config, IFlightRepository flightRepository)
        {
            _httpClient = httpClient;
            _apiKey = config["Skyscanner:ApiKey"]!;
            _httpClient.DefaultRequestHeaders.Add("x-rapidapi-key", _apiKey);
            _httpClient.DefaultRequestHeaders.Add("x-rapidapi-host", "skyscanner89.p.rapidapi.com");
            _flightRepository = flightRepository;
        }

        public async Task<IEnumerable<Flight>> GetFlightsAsync(string origin, string destination, DateTime departureDate, DateTime returnDate)
        {
            var results = new List<Flight>();

            var alreadySearchedDeparture = await _flightRepository.GetSameFlightInfoAsync(departureDate, origin, destination);
            var alreadySearchedReturn = await _flightRepository.GetSameFlightInfoAsync(returnDate, origin, destination);
            if (alreadySearchedDeparture is null)
            {
                var url = $"https://skyscanner89.p.rapidapi.com/flights/one-way/list?origin={origin}&destination={destination}&departureDate={departureDate}";

                var response = await _httpClient.GetAsync(url);

                // Get flight entity from response
            }
            else
            {
                foreach(var flight in alreadySearchedDeparture)
                {
                    results.Add(flight);
                }
            }
            if (alreadySearchedReturn is null)
            {
                var url = $"https://skyscanner89.p.rapidapi.com/flights/one-way/list?origin={origin}&destination={destination}&departureDate={returnDate}";

                var response = await _httpClient.GetAsync(url);

                // Get flight entity from response
            }
            else
            {
                foreach(var flight in alreadySearchedReturn)
                {
                    results.Add(flight);
                }
            }
            return results;
        }
    }
}
