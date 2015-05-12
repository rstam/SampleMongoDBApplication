using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace WebApplication.Models
{
    public class Flight
    {
        public ObjectId Id { get; set; }
        [BsonElement("YEAR")]
        public int Year { get; set; }
        [BsonElement("QUARTER")]
        public int Quarter { get; set; }
        [BsonElement("DAY_OF_MONTH")]
        public int DayOfMonth { get; set; }
        [BsonElement("DAY_OF_YEAR")]
        public int DayOfWeek { get; set; }
        [BsonElement("FL_DATE")]
        public DateTime FlightDate { get; set; }
        [BsonElement("UNIQUE_CARRIER")]
        public string UniqueCarrier { get; set; }
        [BsonElement("AIRLINE_ID")]
        public int AirlineId { get; set; }
        [BsonElement("CARRIER")]
        public string Carrier { get; set; }
        [BsonElement("FL_NUM")]
        public int FlightNumber { get; set; }
        [BsonElement("ORIGIN_AIRPORT_ID")]
        public int OriginAirportId { get; set; }
        [BsonElement("ORIGIN_AIRPORT_SEQ_ID")]
        public int OriginAirportSeqId { get; set; }
        [BsonElement("ORIGIN_CITY_MARKET_ID")]
        public int OriginCityMarketId { get; set; }
        [BsonElement("ORIGIN")]
        public string Origin { get; set; }
        [BsonElement("ORIGIN_CITY_NAME")]
        public string OriginCityName { get; set; }
        [BsonElement("ORIGIN_STATE_ABR")]
        public string OriginStateAbbreviation { get; set; }
        [BsonElement("DEST_AIRPORT_ID")]
        public int DestinationAirportId { get; set; }
        [BsonElement("DEST_AIRPORT_SEQ_ID")]
        public int DestinationAirportSeqId { get; set; }
        [BsonElement("DEST_CITY_MARKET_ID")]
        public int DestinationCityMarketId { get; set; }
        [BsonElement("DEST")]
        public string Destination { get; set; }
        [BsonElement("DEST_CITY_NAME")]
        public string DestinationCityName { get; set; }
        [BsonElement("DEST_STATE_ABR")]
        public string DestinationStateAbbreviation { get; set; }
        [BsonElement("CRS_DEP_TIME")]
        public string ScheduledDepartureTime { get; set; }
        [BsonElement("DEP_TIME")]
        public string ActualDepartureTime { get; set; }
        [BsonElement("DEP_DELAY")]
        public double DepartureDelay { get; set; }
        [BsonElement("CRS_ARR_TIME")]
        public string ScheduledArrivalTime { get; set; }
        [BsonElement("ARR_TIME")]
        public string ActualArrivalTime { get; set; }
        [BsonElement("ARR_DELAY")]
        public double ArrivalDelay { get; set; }
        [BsonElement("CANCELLED")]
        public bool Cancelled { get; set; }
        [BsonElement("CANCELLATION_CODE")]
        public string CancellationCode { get; set; }
        [BsonElement("DIVERTED")]
        public bool Diverted { get; set; }
        [BsonElement("DISTANCE")]
        public double Distance { get; set; }
        [BsonElement("CARRIER_DELAY")]
        public double CarrierDelay { get; set; }
        [BsonElement("WEATHER_DELAY")]
        public double WeatherDelay { get; set; }
        [BsonElement("NAS_DELAY")]
        public double NasDelay { get; set; }
        [BsonElement("SECURITY_DELAY")]
        public double SecurityDelay { get; set; }
        [BsonElement("LATE_AIRCRAFT_DELAY")]
        public double LateAircraftDelay { get; set; }
    }
}