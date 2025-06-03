using Microsoft.Identity.Client;
using System;
using System.Text.Json;
using TravelPlanner.Application.DTOs;
using TravelPlanner.Core.Entities;
using TravelPlanner.Core.Interfaces.Repositories;
using TravelPlanner.Core.Interfaces.Services;

namespace TravelPlanner.Application.Services
{
    public class FlightTwoService : IFlightTwoService
    {
        private readonly HttpClient _httpClient;
        private readonly IAITAService _aitaService;
        private readonly IFlightTwoRepository _flightTwoRepository;
        private readonly IAmadeusTokenRepository _amadeusTokenRepository;

        public FlightTwoService(HttpClient httpClient, IAITAService aitaService, IFlightTwoRepository flightTwoRepository,
            IAmadeusTokenRepository amadeusTokenRepository)
        {
            _httpClient = httpClient;
            _aitaService = aitaService;
            _flightTwoRepository = flightTwoRepository;
            _amadeusTokenRepository = amadeusTokenRepository;
        }
        public async Task<IEnumerable<FlightTwo>> GetFlightsTwoAsync(FlightTwoRequest request)
        {
            List<FlightTwo> flights = new List<FlightTwo>();

            var origin = await _aitaService.GetAITACodeAsync(new AITARequest { City = request.OriginCity });
            await Task.Delay(500);
            var destination = await _aitaService.GetAITACodeAsync(new AITARequest { City = request.DestinationCity });
            var token = await _amadeusTokenRepository.GetCurrentTokenAsync();

            var SavedDepartureFlights = await _flightTwoRepository.GetFlightsTwoInfoAsync(request.DepartureDate, origin, destination, request.NumberOfPassengers);

            if (SavedDepartureFlights.Any())
            {
                flights.AddRange(SavedDepartureFlights);
            }
            else
            {
                var url = $"https://test.api.amadeus.com/v2/shopping/flight-offers?" +
                    $"originLocationCode={origin}" +
                    $"&destinationLocationCode={destination}" +
                    $"&departureDate={request.DepartureDate:yyyy-MM-dd}" +
                    $"&adults={request.NumberOfPassengers}" +
                    $"&max=9";

                _httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token.AccessToken);

                var response = await _httpClient.GetAsync(url);
                response.EnsureSuccessStatusCode();

                var content = await response.Content.ReadAsStringAsync();
                Console.WriteLine(content);

                using var jsonDoc = JsonDocument.Parse(content);
                var root = jsonDoc.RootElement;

                foreach (var offer in root.GetProperty("data").EnumerateArray().Take(3))
                {
                    var itinerary = offer.GetProperty("itineraries")[0];
                    var segments = itinerary.GetProperty("segments");
                    var segmentsArray = segments.EnumerateArray().ToArray();
                    var firstSegment = segmentsArray[0];
                    var lastSegment = segmentsArray[segmentsArray.Length - 1];

                    var departure = firstSegment.GetProperty("departure");
                    var arrival = lastSegment.GetProperty("arrival");

                    var priceAmount = offer.GetProperty("price").GetProperty("total").GetString();
                    var priceCurrency = offer.GetProperty("price").GetProperty("currency").GetString();

                    var flight = new FlightTwo
                    {
                        OriginCity = request.OriginCity,
                        OriginAirport = departure.GetProperty("iataCode").GetString(),
                        DestinationCity = request.DestinationCity,
                        DestinationAirport = arrival.GetProperty("iataCode").GetString(),
                        Date = DateTime.Parse(departure.GetProperty("at").GetString()),
                        Price = priceAmount + priceCurrency,
                        Passengers = request.NumberOfPassengers
                    };

                    await _flightTwoRepository.AddAsync(flight);
                    flights.Add(flight);
                }

                await _flightTwoRepository.SaveFlightAsync();
            }

            var SavedArrivalFlights = await _flightTwoRepository.GetFlightsTwoInfoAsync(request.ArrivalDate, destination, origin, request.NumberOfPassengers);

            if (SavedArrivalFlights.Any())
            {
                flights.AddRange(SavedArrivalFlights);
            }

            else
            {
                var secondUrl = $"https://test.api.amadeus.com/v2/shopping/flight-offers?" +
                    $"originLocationCode={destination}" +
                    $"&destinationLocationCode={origin}" +
                    $"&departureDate={request.ArrivalDate:yyyy-MM-dd}" +
                    $"&adults={request.NumberOfPassengers}" +
                    $"&max=9";

                _httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token.AccessToken);

                var response = await _httpClient.GetAsync(secondUrl);
                response.EnsureSuccessStatusCode();

                var content = await response.Content.ReadAsStringAsync();

                using var jsonDoc = JsonDocument.Parse(content);
                var root = jsonDoc.RootElement;

                foreach (var offer in root.GetProperty("data").EnumerateArray().Take(3))
                {
                    var itinerary = offer.GetProperty("itineraries")[0];
                    var segments = itinerary.GetProperty("segments");
                    var segmentsArray = segments.EnumerateArray().ToArray();
                    var firstSegment = segmentsArray[0];
                    var lastSegment = segmentsArray[segmentsArray.Length - 1];

                    var departure = firstSegment.GetProperty("departure");
                    var arrival = lastSegment.GetProperty("arrival");

                    var priceAmount = offer.GetProperty("price").GetProperty("total").GetString();
                    var priceCurrency = offer.GetProperty("price").GetProperty("currency").GetString();

                    var flight = new FlightTwo
                    {
                        OriginCity = request.DestinationCity,
                        OriginAirport = departure.GetProperty("iataCode").GetString(),
                        DestinationCity = request.OriginCity,
                        DestinationAirport = arrival.GetProperty("iataCode").GetString(),
                        Date = DateTime.Parse(departure.GetProperty("at").GetString()),
                        Price = priceAmount + priceCurrency,
                        Passengers = request.NumberOfPassengers
                    };

                    await _flightTwoRepository.AddAsync(flight);
                    flights.Add(flight);
                }
                await _flightTwoRepository.SaveFlightAsync();
            }

            return flights;
        }
    }
}
