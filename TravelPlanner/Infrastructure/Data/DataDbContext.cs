using Microsoft.EntityFrameworkCore;
using TravelPlanner.Core.Entities;

namespace TravelPlanner.Infrastructure.Data
{
    public class DataDbContext : DbContext
    {
        public DataDbContext(DbContextOptions<DataDbContext> options) : base(options) { }

        public DbSet<Destination> Destinations { get; set; }
        public DbSet<Weather> Weathers { get; set; }
        public DbSet<Flight> Flights { get; set; }
        public DbSet<AirportInformation> AirportInformations { get; set; }
        public DbSet<Hotel> Hotels { get; set; }
        public DbSet<HotelDestination> HotelDestinations { get; set; }
        public DbSet<AmadeusToken> AmadeusTokens { get; set; }
        public DbSet<AITA> AITAs { get; set; }
        public DbSet<FlightTwo> FlightTwos { get; set; }
        public DbSet<HotelInfoTwo> HotelInfoTwos { get; set; }
        public DbSet<HotelTwo> HotelTwos { get; set; }
    }
}
