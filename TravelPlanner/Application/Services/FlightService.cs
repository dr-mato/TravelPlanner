using System.Text.Json;
using TravelPlanner.Application.DTOs;
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
        private readonly IAirportInformationRepository _airportInformationRepository;

        public FlightService(HttpClient httpClient, IConfiguration config, IFlightRepository flightRepository, IAirportInformationRepository airportInformationRepository)
        {
            _httpClient = httpClient;
            _apiKey = config["Skyscanner:ApiKey"]!;
            _httpClient.DefaultRequestHeaders.Add("x-rapidapi-key", _apiKey);
            _httpClient.DefaultRequestHeaders.Add("x-rapidapi-host", "skyscanner89.p.rapidapi.com");
            _flightRepository = flightRepository;
            _airportInformationRepository = airportInformationRepository;
        }

        public async Task<IEnumerable<Flight>> GetFlightsAsync(FlightRequest flight)
        {
            var results = new List<Flight>();

            var alreadySearchedDeparture = await _flightRepository.GetSameFlightInfoAsync(flight.DepartureDate, flight.Origin, flight.Destination);
            var alreadySearchedReturn = await _flightRepository.GetSameFlightInfoAsync(flight.DepartureDate, flight.Origin, flight.Destination);

            var originAirport = await _airportInformationRepository.GetAirportCodeAndIdAsync(flight.Origin);
            var destinationAirport = await _airportInformationRepository.GetAirportCodeAndIdAsync(flight.Destination);
            var originAirportCode = originAirport?.AirportCode;
            var originAirportId = originAirport?.AirportId;
            var destinationAirportCode = destinationAirport?.AirportCode;
            var destinationAirportId = destinationAirport?.AirportId;
            

            if (originAirport is null)
            {
                var url = $"https://skyscanner89.p.rapidapi.com/flights/auto-complete?query={flight.Origin}";

                var response = await _httpClient.GetAsync(url);
                response.EnsureSuccessStatusCode();

                var content = await response.Content.ReadAsStringAsync();
                var jsonDocument = JsonDocument.Parse(content);
                var airportData = jsonDocument.RootElement.GetProperty("inputSuggest").GetProperty("0").
                    GetProperty("RelevantFlightParams");

                originAirportCode = airportData.GetProperty("skyId").GetString();
                originAirportId = airportData.GetProperty("entityId").GetString();

                var airportInformation = new AirportInformation
                {
                    City = flight.Origin,
                    AirportCode = originAirportCode,
                    AirportId = originAirportId
                };

                await _airportInformationRepository.AddAsync(airportInformation);

            }
            if (destinationAirport is null)
            {
                var url = $"https://skyscanner89.p.rapidapi.com/flights/auto-complete?query={flight.Destination}";

                var response = await _httpClient.GetAsync(url);
                response.EnsureSuccessStatusCode();

                var content = await response.Content.ReadAsStringAsync();
                var jsonDocument = JsonDocument.Parse(content);
                var airportData = jsonDocument.RootElement.GetProperty("inputSuggest").GetProperty("0").
                    GetProperty("RelevantFlightParams");

                destinationAirportCode = airportData.GetProperty("skyId").GetString();
                destinationAirportId = airportData.GetProperty("entityId").GetString();

                var airportInformation = new AirportInformation
                {
                    City = flight.Destination,
                    AirportCode = destinationAirportCode,
                    AirportId = destinationAirportId
                };

                await _airportInformationRepository.AddAsync(airportInformation);

            }

            if (alreadySearchedDeparture is null)
            {

                var url = $"https://skyscanner89.p.rapidapi.com/flights/one-way/list?origin={originAirportCode}" +
                    $"&originId={originAirportId}&destination={destinationAirportCode}&destinationId={destinationAirportId}" +
                    $"&departureDate={flight.DepartureDate}";

                var response = await _httpClient.GetAsync(url);
                response.EnsureSuccessStatusCode();

                // Get flight entity from response
            }
            else
            {
                foreach(var databaseFlight in alreadySearchedDeparture)
                {
                    results.Add(databaseFlight);
                }
            }
            if (alreadySearchedReturn is null)
            {


                var url = $"https://skyscanner89.p.rapidapi.com/flights/one-way/list?origin={destinationAirportCode}" +
                    $"&originId={destinationAirportId}&destination={originAirportCode}&destinationId={originAirportId}" +
                    $"&departureDate={flight.ReturnDate}";

                var response = await _httpClient.GetAsync(url);
                response.EnsureSuccessStatusCode();

                var content = await response.Content.ReadAsStringAsync();
                var jsonDocument = JsonDocument.Parse(content);

                Console.WriteLine(jsonDocument.RootElement.ToString());


                // Get flight entity from response
            }
            else
            {
                foreach(var databaseFlight in alreadySearchedReturn)
                {
                    results.Add(databaseFlight);
                }
            }
            return results;
        }
    }
}
