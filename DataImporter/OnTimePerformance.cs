using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataImporter
{
    public class OnTimePerformance
    {
        public int? YEAR { get; set; }
        public int? QUARTER { get; set; }
        public int? DAY_OF_MONTH { get; set; }
        public int? DAY_OF_WEEK { get; set; }
        public DateTime? FL_DATE { get; set; }
        public string UNIQUE_CARRIER { get; set; }
        public int? AIRLINE_ID { get; set; }
        public string CARRIER { get; set; }
        public int? FL_NUM { get; set; }
        public int? ORIGIN_AIRPORT_ID { get; set; }
        public int? ORIGIN_AIRPORT_SEQ_ID { get; set; }
        public int? ORIGIN_CITY_MARKET_ID { get; set; }
        public string ORIGIN { get; set; }
        public string ORIGIN_CITY_NAME { get; set; }
        public string ORIGIN_STATE_ABR { get; set; }
        public int? DEST_AIRPORT_ID { get; set; }
        public int? DEST_AIRPORT_SEQ_ID { get; set; }
        public int? DEST_CITY_MARKET_ID { get; set; }
        public string DEST { get; set; }
        public string DEST_CITY_NAME { get; set; }
        public string DEST_STATE_ABR { get; set; }
        public string CRS_DEP_TIME { get; set; }
        public string DEP_TIME { get; set; }
        public double? DEP_DELAY { get; set; }
        public string CRS_ARR_TIME { get; set; }
        public string ARR_TIME { get; set; }
        public double? ARR_DELAY { get; set; }
        public double? CANCELLED { get; set; }
        public string CANCELLATION_CODE { get; set; }
        public double? DIVERTED { get; set; }
        public double? DISTANCE { get; set; }
        public double? CARRIER_DELAY { get; set; }
        public double? WEATHER_DELAY { get; set; }
        public double? NAS_DELAY { get; set; }
        public double? SECURITY_DELAY { get; set; }
        public double? LATE_AIRCRAFT_DELAY { get; set; }
    }
}
