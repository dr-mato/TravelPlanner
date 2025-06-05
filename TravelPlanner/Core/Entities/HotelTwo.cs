namespace TravelPlanner.Core.Entities
{
    public class HotelTwo
    {
        public int Id { get; set; }
        public string City { get; set; }
        public string HotelId { get; set; }
        public string HotelName { get; set; }
        public DateTime CheckInDate { get; set; }
        public DateTime CheckOutDate { get; set; }
        public int NumberOfPeople { get; set; }
        public string HotelDescription { get; set; }
        public string Price { get; set; }
    }
}
