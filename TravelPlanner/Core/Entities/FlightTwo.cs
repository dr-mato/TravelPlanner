namespace TravelPlanner.Core.Entities
{
    public class FlightTwo
    {
        public int Id { get; set; }
        public string OriginCity { get; set; }
        public string OriginAirport { get; set; }
        public string DestinationCity { get; set; }
        public string DestinationAirport { get; set; }
        public DateTime Date { get; set; }
        public string Price { get; set; }
        public int Passengers { get; set; }
        public FlightTwoClass Class { get; set; }
    }
}
