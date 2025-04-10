using System.ComponentModel.DataAnnotations;

namespace TravelPlanner.Application.DTOs
{
    public class WeatherRequest
    {
        [Required]
        public string Location { get; set; }
        [Required]
        public DateTime StartDate { get; set; }
        [Required]
        public DateTime EndDate { get; set; }
    }
}
