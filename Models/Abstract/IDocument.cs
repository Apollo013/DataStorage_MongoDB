using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;

namespace Models.Abstract
{
    /// <summary>
    /// Generic mongo entity interface
    /// </summary>
    /// <typeparam name="TKey"></typeparam>
    public interface IDocument<TKey>
    {
        /// <summary>
        /// Gets or sets the Id for the entity
        /// </summary>
        //[BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        TKey Id { get; set; }

        [BsonRepresentation(BsonType.DateTime)]
        DateTime ModifiedOn { get; }
    }

    /// <summary>
    /// Mongo entity interface that uses a string type for the Id
    /// </summary>
    public interface IDocument : IDocument<string>
    { }

    //public interface IEntity : IEntity<ObjectId>
    //{ }
}
