using DataAccessLayer.Contexts;
using DataAccessLayer.Repositories.Abstract;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Models.Entities;
using TestClient.TestModels;

namespace TestClient.DataTests
{
    /// <summary>
    /// Summary description for TestCollectionName
    /// </summary>
    [TestClass]
    public class RepositoryTest_CollectionName
    {
        public RepositoryTest_CollectionName()
        { }

        [TestMethod]
        public void CollectionNames()
        {
            // Essentially we want to test if the repository is able to pick up on the collection name from the 'CollectionNameAttribute'

            // Arrange & Act
            var ctx = new MongoDBContext();
            var repo1 = new MongoRepository<Restaurant>(ctx);
            var repo2 = new MongoRepository<RestaurantWithInheritedAttribute>(ctx);
            var repo3 = new MongoRepository<RestaurantWithAttributeOverride>(ctx);

            // Assert
            Assert.IsTrue(repo1.Collection.CollectionNamespace.CollectionName.Equals("restaurants"));
            Assert.IsTrue(repo2.Collection.CollectionNamespace.CollectionName.Equals("restaurants"));
            Assert.IsTrue(repo3.Collection.CollectionNamespace.CollectionName.Equals("overriderestaurantname"));
        }
    }
}
