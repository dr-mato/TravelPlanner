namespace TravelPlanner.Core.Entities
{
    public class AmadeusToken
    {
        public int Id { get; set; }
        public string AccessToken { get; set; }
        public DateTime ExpirationTime { get; set; }
    }
}
