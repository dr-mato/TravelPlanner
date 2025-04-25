using System.Globalization;
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
            var alreadySearchedReturn = await _flightRepository.GetSameFlightInfoAsync(flight.ReturnDate, flight.Origin, flight.Destination);

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
                var airportData = jsonDocument.RootElement.GetProperty("inputSuggest")[0].GetProperty("navigation")
                    .GetProperty("relevantFlightParams");

                originAirportCode = airportData.GetProperty("skyId").GetString();
                originAirportId = airportData.GetProperty("entityId").GetString();

                var airportInformation = new AirportInformation
                {
                    City = flight.Origin,
                    AirportCode = originAirportCode,
                    AirportId = originAirportId
                };

                await _airportInformationRepository.AddAsync(airportInformation);
                await _airportInformationRepository.SaveChangesAsync();

            }

            if (destinationAirport is null)
            {
                var url = $"https://skyscanner89.p.rapidapi.com/flights/auto-complete?query={flight.Destination}";

                var response = await _httpClient.GetAsync(url);
                response.EnsureSuccessStatusCode();

                var content = await response.Content.ReadAsStringAsync();
                var jsonDocument = JsonDocument.Parse(content);
                var airportData = jsonDocument.RootElement.GetProperty("inputSuggest")[0].GetProperty("navigation")
                    .GetProperty("relevantFlightParams");

                destinationAirportCode = airportData.GetProperty("skyId").GetString();
                destinationAirportId = airportData.GetProperty("entityId").GetString();

                var airportInformation = new AirportInformation
                {
                    City = flight.Destination,
                    AirportCode = destinationAirportCode,
                    AirportId = destinationAirportId
                };

                await _airportInformationRepository.AddAsync(airportInformation);
                await _airportInformationRepository.SaveChangesAsync();

            }

            if (!alreadySearchedDeparture.Any())
            {

                var url = $"https://skyscanner89.p.rapidapi.com/flights/one-way/list?origin={originAirportCode}" +
                    $"&originId={originAirportId}&destination={destinationAirportCode}&destinationId={destinationAirportId}" +
                    $"&departureDate={flight.DepartureDate}";

                var response = await _httpClient.GetAsync(url);
                response.EnsureSuccessStatusCode();

                var content = await response.Content.ReadAsStringAsync();
                
                using var jsonDocument = JsonDocument.Parse(content);
                
                var flightData = GetBestFlightMatch(jsonDocument, flight.DepartureDate);
                
                results.Add(flightData);
                await _flightRepository.AddAsync(flightData);
                await _flightRepository.SaveChangesAsync();
            }
            else
            {
                foreach(var databaseFlight in alreadySearchedDeparture)
                {
                    results.Add(databaseFlight);
                }
            }

            if (!alreadySearchedReturn.Any())
            {


                var url = $"https://skyscanner89.p.rapidapi.com/flights/one-way/list?origin={destinationAirportCode}" +
                    $"&originId={destinationAirportId}&destination={originAirportCode}&destinationId={originAirportId}" +
                    $"&departureDate={flight.ReturnDate}";

                var response = await _httpClient.GetAsync(url);
                response.EnsureSuccessStatusCode();

                var content = await response.Content.ReadAsStringAsync();

                using var jsonDocument = JsonDocument.Parse(content);

                var flightData = GetBestFlightMatch(jsonDocument, flight.ReturnDate);

                results.Add(flightData);
                await _flightRepository.AddAsync(flightData);
                await _flightRepository.SaveChangesAsync();
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

        public Flight GetBestFlightMatch(JsonDocument jsonDocument, DateTime targetDate)
        {
            // Track best matches
            Flight bestMatch = null;
            int closestDayDifference = int.MaxValue;
            decimal lowestPrice = decimal.MaxValue;

            // Get the results array
            var results = jsonDocument.RootElement
                .GetProperty("data")
                .GetProperty("flightQuotes")
                .GetProperty("results");

            foreach (JsonElement flight in results.EnumerateArray())
            {
                // Extract ID to get date information
                string id = flight.GetProperty("id").GetString();
                string[] idParts = id.Split('*');

                // Parse the departure date from ID (format: 20250526)
                string departureDateStr = idParts[4];
                DateTime flightDate = DateTime.ParseExact(departureDateStr, "yyyyMMdd", CultureInfo.InvariantCulture);

                // Get the price
                decimal price = flight.GetProperty("content").GetProperty("rawPrice").GetDecimal();

                // Calculate how close this date is to our target
                int dayDifference = Math.Abs((flightDate - targetDate).Days);

                // Determine if this is a better match
                if (dayDifference < closestDayDifference ||
                    (dayDifference == closestDayDifference && price < lowestPrice))
                {
                    closestDayDifference = dayDifference;
                    lowestPrice = price;

                    // Extract remaining flight details
                    string origin = idParts[2];
                    string destination = idParts[3];
                    string carrierCode = idParts[6];

                    // Create the flight object
                    bestMatch = new Flight
                    {
                        FlightNumber = $"{carrierCode}{new Random().Next(100, 999)}", // Simple flight number
                        Origin = origin,
                        Destination = destination,
                        DepartureDate = flightDate,
                        ReturnDate = flightDate.AddDays(7), // need to remove this
                        Airline = string.Empty, // need to remove this
                        Price = price
                    };
                }
            }

            return bestMatch;
        }
    }
}
