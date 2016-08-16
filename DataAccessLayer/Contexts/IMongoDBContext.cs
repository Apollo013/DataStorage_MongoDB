using MongoDB.Driver;

namespace DataAccessLayer.Contexts
{
    public interface IMongoDBContext
    {
        IMongoDatabase Database { get; }
    }
}
