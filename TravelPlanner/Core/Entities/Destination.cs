using System.ComponentModel.DataAnnotations;

namespace TravelPlanner.Core.Entities
{
    public class Destination
    {
        [Key]
        public int Id { get; set; }       // Unique identifier (for database)
        [Required]
        public string Name { get; set; }  // e.g., "Bali, Indonesia"
        [Required]
        public string Country { get; set; }
        [Required]
        public string Summary { get; set; }
        public decimal BudgetEstimate { get; set; }
        public bool WeatherMatch { get; set; }
    }
}
