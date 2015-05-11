using MongoDB.Bson.Serialization.Attributes;

namespace DataImporter
{
    public class Airline
    {
        [BsonId]
        public int Code { get; set; }
        public string Description { get; set; }
    }
}
