using System.Text.Json.Serialization;
using TravelPlanner.Core.Entities;

namespace TravelPlanner.Application.DTOs
{
    public class HotelListTwoRequest
    {
        public string City { get; set; }
        public List<Amenity> Amenities { get; set; }
        public int[] Stars { get; set; }
    }
}
