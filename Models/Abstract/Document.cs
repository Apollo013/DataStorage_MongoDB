using Models.Entities;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;

namespace Models.Abstract
{
    /// <summary>
    /// Base class for mongodb documents
    /// </summary>
    [BsonKnownTypes(typeof(Restaurant))]
    public class Document : IDocument
    {
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

        [BsonRepresentation(BsonType.DateTime)]
        public DateTime ModifiedOn { get { return DateTime.Now; } }
    }
}
