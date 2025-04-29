namespace TravelPlanner.Core.Entities
{
    public class DailyPlan
    {
        public int DayNumber { get; set; }
        public string MorningActivity { get; set; }
        public string AfternoonActivity { get; set; }
        public string EveningActivity { get; set; }
        public string Notes { get; set; }
    }
}
