using Microsoft.EntityFrameworkCore;
using TravelPlanner.Core.Entities;

namespace TravelPlanner.Infrastructure.Data
{
    public class DataDbContext : DbContext
    {
        public DataDbContext(DbContextOptions<DataDbContext> options) : base(options) { }

        public DbSet<Destination> Destinations { get; set; }
        public DbSet<Weather> Weathers { get; set; }
    }
}
