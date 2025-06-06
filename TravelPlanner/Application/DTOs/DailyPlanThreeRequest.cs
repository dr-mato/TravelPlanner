using TravelPlanner.Core.Entities;

namespace TravelPlanner.Application.DTOs
{
    public class DailyPlanThreeRequest
    {
        public Destination Destination { get; set; }
        public List<Weather> Weathers { get; set; }
        public List<FlightTwo> Flights { get; set; }
        public HotelTwo Hotel { get; set; }
    }
}
