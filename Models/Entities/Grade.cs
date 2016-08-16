using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;

namespace Models.Entities
{
    public class RestaurantGrade
    {
        [BsonElement("date")]
        [BsonRepresentation(BsonType.DateTime)]
        public DateTime? Date { get; set; }

        [BsonElement("grade")]
        public string Grade { get; set; }

        [BsonElement("score")]
        public int? Score { get; set; } = 0;
    }
}
