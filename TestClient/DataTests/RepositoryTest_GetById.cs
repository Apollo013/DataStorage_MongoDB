using DataAccessLayer.Contexts;
using DataAccessLayer.Repositories.Abstract;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Models.Entities;

namespace TestClient.DataTests
{
    [TestClass]
    public class RepositoryTest_GetById
    {
        private MongoRepository<Restaurant> repo;

        public RepositoryTest_GetById()
        {
            // Arrange
            repo = new MongoRepository<Restaurant>(new MongoDBContext());
        }

        [TestMethod]
        public void GetById()
        {
            // Act
            var restaurant = repo.GetById("57aca55c6a9b17bcb5aa5d98");

            // Assert
            restaurant.Should().NotBeNull();
            restaurant.Name.Should().Be("Dj Reynolds Pub And Restaurant");
        }
    }
}
