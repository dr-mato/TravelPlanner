using System.Text.Json;
using TravelPlanner.Application.DTOs;
using TravelPlanner.Core.Entities;
using TravelPlanner.Core.Interfaces.Repositories;
using TravelPlanner.Core.Interfaces.Services;

namespace TravelPlanner.Application.Services
{
    public class HotelListTwoService : IHotelListTwoService
    {
        private readonly HttpClient _httpClient;
        private readonly IAITAService _aitaService;
        private readonly IHotelInfoTwoRepository _hotelInfoTwoRepository;
        private readonly IAmadeusTokenRepository _amadeusTokenRepository;

        public HotelListTwoService(HttpClient httpClient, IAITAService aitaService, IHotelInfoTwoRepository hotelInfoTwoRepository,
            IAmadeusTokenRepository amadeusTokenRepository)
        {
            _httpClient = httpClient;
            _aitaService = aitaService;
            _hotelInfoTwoRepository = hotelInfoTwoRepository;
            _amadeusTokenRepository = amadeusTokenRepository;
        }

        public async Task<IEnumerable<HotelInfoTwo>> GetHotelInfoByCityAsync(HotelListTwoRequest request)
        {
            var savedHotels = await _hotelInfoTwoRepository.GetHotels(request);
            var aitaCode = await _aitaService.GetAITACodeAsync(new AITARequest { City = request.City });

            if (savedHotels.Any())
            {
                return savedHotels;
            }
            else
            {
                var token = await _amadeusTokenRepository.GetCurrentTokenAsync();

                var amenitiesQuery = request.Amenities != null && request.Amenities.Any()
                    ? $"&amenities={string.Join(",", request.Amenities)}"
                    : "";

                var ratingsQuery = request.Stars != null && request.Stars.Any()
                    ? $"&ratings={string.Join(",", request.Stars.Select(s => s.ToString()))}"
                    : "";


                var url = $"https://test.api.amadeus.com/v1/reference-data/locations/hotels/by-city?cityCode={aitaCode}{amenitiesQuery}{ratingsQuery}";

                _httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token.AccessToken);

                var response = await _httpClient.GetAsync(url);
                response.EnsureSuccessStatusCode();

                var content = await response.Content.ReadAsStringAsync();

                using var jsonDoc = JsonDocument.Parse(content);
                var root = jsonDoc.RootElement;

                var hotels = new List<HotelInfoTwo>();
                
                foreach(var hotel in root.GetProperty("data").EnumerateArray().Take(3))
                {
                    var hotelInfo = new HotelInfoTwo
                    {
                        Name = hotel.GetProperty("name").GetString(),
                        HotelId = hotel.GetProperty("hotelId").GetString(),
                        City = request.City,
                        Amenities = request.Amenities ?? [],
                        Stars = request.Stars ?? Array.Empty<int>()
                    };

                    hotels.Add(hotelInfo);
                    await _hotelInfoTwoRepository.AddAsync(hotelInfo);
                }

                await _hotelInfoTwoRepository.SaveHotelInfoAsync();

                return hotels;
            }
        }
    }
}
