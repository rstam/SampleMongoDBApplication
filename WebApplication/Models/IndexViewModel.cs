using System;
using System.Collections.Generic;

namespace WebApplication.Models
{
    public class IndexViewModel
    {
        public IEnumerable<Airline> Airlines { get; set; }
        public IEnumerable<Airport> Airports { get; set; }
    }
}
