
namespace WebApplication.Models
{
    public class SearchCriteriaModel
    {
        public string FromDate { get; set; }
        public string ToDate { get; set; }
        public int? AirlineId { get; set; }
        public int? OriginId { get; set; }
        public int? DestinationId { get; set; }
    }
}