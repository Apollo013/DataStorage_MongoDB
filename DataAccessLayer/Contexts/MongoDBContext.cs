using MongoDB.Driver;
using System;
using System.Configuration;

namespace DataAccessLayer.Contexts
{
    public class MongoDBContext : IMongoDBContext
    {
        #region PROPERTIES
        public IMongoDatabase Database { get; private set; }
        #endregion

        #region CONSTRUCTORS
        public MongoDBContext() : this(GetDefaultConnectionString())
        { }

        public MongoDBContext(string connectionString) : this(new MongoUrl(connectionString))
        { }

        public MongoDBContext(MongoUrl url)
        {
            // Defense
            if (url == null)
            {
                throw new ArgumentNullException("Url must be provided");
            }

            var client = new MongoClient(url);
            Database = client.GetDatabase(url.DatabaseName);
        }

        #endregion

        #region HELPERS
        /// <summary>
        /// Attempts to retrieve the connection string from the 'connectionStrings' section of the configuration file.
        /// </summary>
        /// <returns></returns>
        private static string GetDefaultConnectionString()
        {
            var conn = ConfigurationManager.ConnectionStrings["DefaultMongoConnection"].ConnectionString;
            if (string.IsNullOrWhiteSpace(conn))
            {
                throw new SettingsPropertyNotFoundException("Connection string not found");
            }
            return conn;
        }
        #endregion
    }
}
