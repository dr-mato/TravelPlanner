namespace TravelPlanner.Core.Entities
{
    public class HotelInfoTwo
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string HotelId { get; set; }
        public string City { get; set; }
        public List<Amenity> Amenities { get; set; }
        public int[] Stars { get; set; }
    }
}
