using System.ComponentModel.DataAnnotations;

namespace TravelPlanner.Core.Entities
{
    public class HotelDestination
    {
        public string City { get; set; }
        [Required]
        [Key]
        public string DestinationId { get; set; }
        public string SearchType { get; set; }
    }
}
