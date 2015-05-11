
namespace WebApplication.Models
{
    public class SearchResultViewModel
    {
        public int TotalNumberOfFlights { get; set; }
        public int TotalNumberOfDelayedFlights { get; set; }
        public double AverageDelayInMinutes { get; set; }
    }
}