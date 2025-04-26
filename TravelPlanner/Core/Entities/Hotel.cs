namespace TravelPlanner.Core.Entities
{
    public class Hotel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int StarRating { get; set; }
        public decimal Price { get; set; }
        public string Currency { get; set; }
        public decimal ReviewScore { get; set; }
        public string ReviewQualityWord { get; set; }
        public int NumberOfReviews { get; set; }
        public string City { get; set; }
        public string LocationArea { get; set; }
        public DateTime CheckInDate { get; set; }
        public DateTime CheckOutDate { get; set; }
    }
}
