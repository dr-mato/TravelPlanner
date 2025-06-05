using System.Text.Json;
using TravelPlanner.Application.DTOs;
using TravelPlanner.Core.Entities;
using TravelPlanner.Core.Interfaces.Repositories;
using TravelPlanner.Core.Interfaces.Services;

namespace TravelPlanner.Application.Services
{
    public class HotelTwoService : IHotelTwoService
    {
        private readonly HttpClient _httpClient;
        private readonly IHotelListTwoService _hotelListTwoService;
        private readonly IAmadeusTokenRepository _amadeusTokenRepository;
        private readonly IHotelTwoRepository _hotelTwoRepository;

        public HotelTwoService(HttpClient httpClient, IHotelListTwoService hotelListTwoService,
            IAmadeusTokenRepository amadeusTokenRepository, IHotelTwoRepository hotelTwoRepository)
        {
            _httpClient = httpClient;
            _hotelListTwoService = hotelListTwoService;
            _amadeusTokenRepository = amadeusTokenRepository;
            _hotelTwoRepository = hotelTwoRepository;
        }

        public async Task<IEnumerable<HotelTwo>> GetHotelsAsync(HotelTwoRequest request)
        {
            var savedHotels = await _hotelTwoRepository.GetHotels(request);
            if (savedHotels.Any())
            {
                return savedHotels;
            }
            else
            {
                var hotelList = await _hotelListTwoService.GetHotelInfoByCityAsync(
                    new HotelListTwoRequest { City = request.City, Amenities = [], Stars = [] });

                var token = await _amadeusTokenRepository.GetCurrentTokenAsync();

                var hotelIds = hotelList.Select(h => h.HotelId).Distinct().ToList();
                var hotelIdsParam = string.Join(",", hotelIds);

                var url = $"https://test.api.amadeus.com/v3/shopping/hotel-offers" +
                          $"?hotelIds={hotelIdsParam}" +
                          $"&checkInDate={request.CheckInDate:yyyy-MM-dd}" +
                          $"&checkOutDate={request.CheckOutDate:yyyy-MM-dd}" +
                          $"&adults={request.NumberOfPeople}";

                var requestMessage = new HttpRequestMessage(HttpMethod.Get, url);
                requestMessage.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token.AccessToken);

                var response = await _httpClient.SendAsync(requestMessage);
                response.EnsureSuccessStatusCode();

                var content = await response.Content.ReadAsStringAsync();
                Console.WriteLine(content);

                using var jsonDoc = JsonDocument.Parse(content);
                var root = jsonDoc.RootElement;

                var hotels = new List<HotelTwo>();
                foreach (var hotel in root.GetProperty("data").EnumerateArray())
                {
                    var hotelData = hotel.GetProperty("hotel");
                    var description = hotel.GetProperty("offers")[0].GetProperty("room")
                        .GetProperty("description").GetProperty("text").GetString();
                    var price = hotel.GetProperty("offers")[0].GetProperty("price").GetProperty("total").GetString();
                    var currency = hotel.GetProperty("offers")[0].GetProperty("price").GetProperty("currency").GetString();

                    var currentHotel = new HotelTwo
                    {
                        City = request.City,
                        HotelId = hotelData.GetProperty("hotelId").GetString(),
                        HotelName = hotelData.GetProperty("name").GetString(),
                        CheckInDate = request.CheckInDate.Date,
                        CheckOutDate = request.CheckOutDate.Date,
                        NumberOfPeople = request.NumberOfPeople,
                        HotelDescription = description,
                        Price = price+currency
                    };
                    
                    hotels.Add(currentHotel);
                    await _hotelTwoRepository.AddAsync(currentHotel);
                }
                await _hotelTwoRepository.SaveHotelsAsync();

                return hotels;
            }
        }
    }
}
