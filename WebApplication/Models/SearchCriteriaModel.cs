using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApplication.Models
{
    public class SearchCriteriaModel
    {
        public string From { get; set; }
        public string To { get; set; }
        public string Carrier { get; set; }
        public string Origin { get; set; }
        public string Destination { get; set; }
    }
}