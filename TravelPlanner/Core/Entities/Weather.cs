using System.ComponentModel.DataAnnotations;

namespace TravelPlanner.Core.Entities
{
    public class Weather
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Location { get; set; }

        [Required]
        public DateTime Date { get; set; }

        public double Temperature { get; set; }
        public string Description { get; set; }
    }
}
