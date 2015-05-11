using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApplication.Models
{
    public class SearchResultViewModel
    {
        public int TotalNumberOfFlights { get; set; }
        public int TotalNumberOfDelayedFlights { get; set; }
        public TimeSpan AverageDelay { get; set; }
    }
}