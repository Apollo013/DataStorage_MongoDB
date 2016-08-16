using DataAccessLayer.Contexts;
using DataAccessLayer.Repositories.Abstract;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Models.Entities;

namespace TestClient.DataTests
{
    [TestClass]
    public class RepositoryTest_Last
    {
        private MongoRepository<Restaurant> repo;

        public RepositoryTest_Last()
        {
            // Arrange
            repo = new MongoRepository<Restaurant>(new MongoDBContext());
        }

        [TestMethod]
        public void Last()
        {
            // Act
            var res = repo.Last();

            // Assert
            res.Should().NotBeNull();
            System.Console.WriteLine(res.Name);
        }

        [TestMethod]
        public void Last_ExpressionFiltering()
        {
            // Act
            var res = repo.Last(i => i.Borough.Equals("Manhattan"));

            // Assert
            res.Should().NotBeNull();
            res.Borough.Should().Be("Manhattan");
            System.Console.WriteLine(res.Name);
        }

        [TestMethod]
        public void Last_DynamicLinqFiltering()
        {
            // Act
            var res = repo.Last("borough.Equals(\"Manhattan\")");

            // Assert
            res.Should().NotBeNull();
            res.Borough.Should().Be("Manhattan");
            System.Console.WriteLine(res.Name);
        }

        [TestMethod]
        public void LastOrdered()
        {
            // Act
            var res = repo.Last((i => i.Id));

            // Assert
            res.Should().NotBeNull();
            System.Console.WriteLine(res.Name);
        }
    }
}
