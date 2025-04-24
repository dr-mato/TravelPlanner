using System.ComponentModel.DataAnnotations;

namespace TravelPlanner.Application.DTOs
{
    public class FlightRequest
    {
        [Required]
        public string Origin { get; set; }
        [Required]
        public string Destination { get; set; }
        public DateTime DepartureDate { get; set; }
        public DateTime ReturnDate { get; set; }
    }
}
