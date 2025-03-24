using System.ComponentModel.DataAnnotations;

namespace TravelPlanner.Application.DTOs
{
    public class UserPreferences
    {
        [Required]
        [Range(0, 10000)]
        public decimal Budget { get; set; }

        [Required]
        public List<string> Interests { get; set; } = new();

        [Required]
        public string TravelStyle { get; set; } = "solo"; // solo, family, luxury

        public string? WeatherPreference { get; set; } // sunny, rainy, etc.
    }
}
