using Models.Abstract;
using Models.Attributes;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System.Collections.Generic;

namespace Models.Entities
{
    [BsonIgnoreExtraElements]
    //[DataContract]
    //[BsonDiscriminator("restaurants")]
    [CollectionName("restaurants")]
    public class Restaurant : Document
    {
        [BsonElement("address")]
        public Address Address { get; set; }

        [BsonIgnoreIfNull]
        [BsonElement("grades")]
        public List<RestaurantGrade> Grades { get; set; }

        [BsonIgnoreIfNull]
        [BsonElement("borough")]
        [BsonRepresentation(BsonType.String)]
        public string Borough { get; set; }

        [BsonRepresentation(BsonType.String)]
        [BsonElement("cuisine")]
        public string Cuisine { get; set; }

        [BsonRepresentation(BsonType.String)]
        [BsonElement("name")]
        public string Name { get; set; }

        [BsonRepresentation(BsonType.String)]
        [BsonElement("restaurant_id")]
        public string RestaurantId { get; set; }
    }
}
