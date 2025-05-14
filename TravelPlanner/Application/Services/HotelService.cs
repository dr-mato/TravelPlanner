using System.Globalization;
using System.Text.Json;
using TravelPlanner.Application.DTOs;
using TravelPlanner.Core.Entities;
using TravelPlanner.Core.Interfaces.Repositories;
using TravelPlanner.Core.Interfaces.Services;
using TravelPlanner.Infrastructure.Repositories;

namespace TravelPlanner.Application.Services
{
    public class HotelService : IHotelService
    {
        private readonly HttpClient _httpClient;
        private readonly string _apiKey;
        private readonly IHotelRepository _hotelRepository;
        private readonly IHotelDestinationRepository _hotelDestinationRepository;

        public HotelService(HttpClient httpClient, IConfiguration config, IHotelRepository hotelRepository,
            IHotelDestinationRepository hotelDestinationRepository)
        {
            _httpClient = httpClient;
            _apiKey = config["Skyscanner:ApiKey"]!;
            _httpClient.DefaultRequestHeaders.Add("x-rapidapi-key", _apiKey);
            _httpClient.DefaultRequestHeaders.Add("x-rapidapi-host", "booking-com15.p.rapidapi.com");
            _hotelRepository = hotelRepository;
            _hotelDestinationRepository = hotelDestinationRepository;
        }

        public async Task<List<Hotel>> GetHotelsAsync(HotelRequest request)
        {
            var results = new List<Hotel>();

            var alreadySearchedHotel = await _hotelDestinationRepository.GetHotelInformationByCity(request.City);

            var destinationId = string.Empty;
            var searchType = string.Empty;
            
            if (alreadySearchedHotel is null)
            {
                var info = await GetDestinationIdAndSearchType(request.City);

                destinationId = info.DestinationId;
                searchType = info.SearchType;
            }
            else
            {
                destinationId = alreadySearchedHotel.DestinationId;
                searchType = alreadySearchedHotel.SearchType;
            }

            var alreadySearchedHotelByCityAndDate = await _hotelRepository.GetHotelsByCityAndDate(request.City, request.ArrivalDate, request.DepartureDate);

            if(!alreadySearchedHotelByCityAndDate.Any())
            {
                var url = $"https://booking-com15.p.rapidapi.com/api/v1/hotels/searchHotels?" +
                          $"dest_id={destinationId}&search_type={searchType}&arrival_date={request.ArrivalDate:yyyy-MM-dd}" +
                          $"&departure_date={request.DepartureDate:yyyy-MM-dd}&adults=1&room_qty=1&page_number=1&units=metric" +
                          $"&temperature_unit=c&languagecode=en-us&currency_code=USD&location=US";

                var response = await _httpClient.GetAsync(url);
                response.EnsureSuccessStatusCode();

                var content = await response.Content.ReadAsStringAsync();
                var jsonDocument = JsonDocument.Parse(content);
                var hotelInfo = jsonDocument.RootElement.GetProperty("data").GetProperty("hotels");

                foreach (var hotel in hotelInfo.EnumerateArray().Take(3))
                {
                    var hotelDetails = new Hotel
                    {
                        Name = hotel.GetProperty("property").GetProperty("name").ToString(),
                        StarRating = hotel.GetProperty("property").GetProperty("accuratePropertyClass").GetInt32(),
                        Price = hotel.GetProperty("property").GetProperty("priceBreakdown")
                            .GetProperty("grossPrice").GetProperty("value").GetDecimal(),
                        Currency = hotel.GetProperty("property").GetProperty("priceBreakdown")
                            .GetProperty("grossPrice").GetProperty("currency").ToString(),
                        ReviewScore = hotel.GetProperty("property")
                            .GetProperty("reviewScore").GetDecimal(),
                        ReviewQualityWord = hotel.GetProperty("property")
                            .GetProperty("reviewScoreWord").ToString(),
                        NumberOfReviews = hotel.GetProperty("property")
                            .GetProperty("reviewCount").GetInt32(),
                        City = request.City,
                        LocationArea = hotel.GetProperty("property")
                            .GetProperty("wishlistName").ToString(),
                        CheckInDate = request.ArrivalDate,
                        CheckOutDate = request.DepartureDate
                    };

                    results.Add(hotelDetails);
                    await _hotelRepository.AddAsync(hotelDetails);
                }
                await _hotelRepository.SaveChangesAsync();
            }
            else
            {
                foreach (var databaseHotel in alreadySearchedHotelByCityAndDate)
                {
                    results.Add(databaseHotel);
                }
            }

            return results;
        }

        private async Task<HotelDestination> GetDestinationIdAndSearchType(string city)
        {
            var url = $"https://booking-com15.p.rapidapi.com/api/v1/hotels/searchDestination?query={city}";

            var response = await _httpClient.GetAsync(url);
            response.EnsureSuccessStatusCode();

            var content = await response.Content.ReadAsStringAsync();
            var jsonDocument = JsonDocument.Parse(content);
            Console.WriteLine(jsonDocument.RootElement.ToString());
            var hotelInfo = jsonDocument.RootElement.GetProperty("data");

            var hotelDestination = new HotelDestination
            {
                City = city,
                DestinationId = hotelInfo[0].GetProperty("dest_id").ToString(),
                SearchType = hotelInfo[0].GetProperty("search_type").ToString()
            };

            await _hotelDestinationRepository.AddAsync(hotelDestination);
            await _hotelDestinationRepository.SaveChangesAsync();

            return hotelDestination;
        }
    }
}
