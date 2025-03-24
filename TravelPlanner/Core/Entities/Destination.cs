namespace TravelPlanner.Core.Entities
{
    public class Destination
    {
        public int Id { get; set; }       // Unique identifier (for database)
        public string Name { get; set; }  // e.g., "Bali, Indonesia"
        public string Summary { get; set; }
        public decimal BudgetEstimate { get; set; }
        public string WeatherMatch { get; set; }
    }
}
