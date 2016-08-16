using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Models.Entities
{
    [BsonNoId]
    public class Address
    {
        [BsonRepresentation(BsonType.String)]
        [BsonElement("building")]
        public string Building { get; set; }

        [BsonElement("coord")]
        public double[] Coord { get; set; }

        [BsonRepresentation(BsonType.String)]
        [BsonElement("street")]
        public string Street { get; set; }

        [BsonRepresentation(BsonType.String)]
        [BsonElement("zipcode")]
        public string Zipcode { get; set; }
    }
}
