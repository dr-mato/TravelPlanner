using TravelPlanner.Core.Entities;

namespace TravelPlanner.Application.DTOs
{
    public class DailyPlanTwoRequest
    {
        public Destination Destination { get; set; }
        public List<Weather> Weathers { get; set; }
        public List<FlightTwo> Flights { get; set; }
        public Hotel Hotel { get; set; }
    }
}
