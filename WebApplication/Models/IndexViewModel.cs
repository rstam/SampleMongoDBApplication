using System;
using System.Collections.Generic;

namespace WebApplication.Models
{
    public class IndexViewModel
    {
        public DateTime? FromDate { get; set; }
        public DateTime? ToDate { get; set; }
        public IEnumerable<Airline> Airlines { get; set; }
        public IEnumerable<Airport> Airports { get; set; }
    }
}
