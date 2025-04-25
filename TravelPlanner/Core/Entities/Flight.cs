using System.ComponentModel.DataAnnotations;

namespace TravelPlanner.Core.Entities
{
    public class Flight
    {
        public int Id { get; set; }
        [Required]
        public string FlightNumber { get; set; }
        [Required]
        public string Origin { get; set; }
        [Required]
        public string Destination { get; set; }
        public DateTime DepartureDate { get; set; }
        public DateTime ReturnDate { get; set; } //need to remove this
        [Required]
        public string Airline { get; set; } //need to remove this
        public decimal Price { get; set; }
    }
}
