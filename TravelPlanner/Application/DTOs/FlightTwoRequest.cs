namespace TravelPlanner.Application.DTOs
{
    public class FlightTwoRequest
    {
        public string OriginCity { get; set; }
        public string DestinationCity { get; set; }
        public DateTime DepartureDate { get; set; }
        public DateTime ArrivalDate { get; set; }
        public int NumberOfPassengers { get; set; }
    }
}
