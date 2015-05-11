using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApplication.Models
{
    public class IndexViewModel
    {
        public DateTime From { get; set; }
        public DateTime To { get; set; }
        public IEnumerable<Carrier> Carriers { get; set; }
        public IEnumerable<Airport> Origins { get; set; }
        public IEnumerable<Airport> Destinations { get; set; }
    }
}
