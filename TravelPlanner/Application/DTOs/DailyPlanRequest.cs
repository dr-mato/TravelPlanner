using TravelPlanner.Core.Entities;

namespace TravelPlanner.Application.DTOs
{
    public class DailyPlanRequest
    {
        public Destination Destination { get; set; }
        public List<Weather> Weathers { get; set; }
        public List<Flight> Flights { get; set; }
        public Hotel Hotel { get; set; }
    }
}
