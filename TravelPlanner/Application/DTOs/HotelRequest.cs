namespace TravelPlanner.Application.DTOs
{
    public class HotelRequest
    {
        public string City { get; set; }
        public DateTime ArrivalDate { get; set; }
        public DateTime DepartureDate { get; set; }
    }
}
