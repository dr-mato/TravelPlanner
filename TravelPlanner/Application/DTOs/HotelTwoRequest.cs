namespace TravelPlanner.Application.DTOs
{
    public class HotelTwoRequest
    {
        public string City { get; set; }
        public int NumberOfPeople { get; set; }
        public DateTime CheckInDate { get; set; }
        public DateTime CheckOutDate { get; set; }
    }
}
