using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace TravelPlanner.Core.Entities
{
    public class AirportInformation
    {
        [Required]
        public string City { get; set; }
        [Required]
        [Key]
        public string AirportCode { get; set; }
        [Required]
        public string AirportId { get; set; }
    }
}
