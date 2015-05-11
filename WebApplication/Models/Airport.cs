using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApplication.Models
{
    public class Airport
    {
        public int AirportId { get; set; }
        public int AirportSeqId { get; set; }
        public int CityMarketId { get; set; }
        public string AirportCode { get; set; }
        public string CityName { get; set; }
        public string StateAbbreviation { get; set; }
    }
}