using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApplication.Models
{
    public class Carrier
    {
        public string UniqueCarrierCode { get; set; }
        public int AirlineId { get; set; }
        public string CarrierCode { get; set; }
        public string CarrierName { get; set; }
    }
}