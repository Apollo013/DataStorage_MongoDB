using DataAccessLayer.Contexts;
using DataAccessLayer.Repositories.Abstract;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Models.Entities;
using System.Threading.Tasks;

namespace TestClient.DataTests
{
    [TestClass]
    public class RepositoryTest_FirstOrDefault
    {
        private MongoRepository<Restaurant> repo;
        private string id = "57aca55c6a9b17bcb5aa5d98";
        private string name = "Dj Reynolds Pub And Restaurant";
        private string street = "West 57 Street";

        public RepositoryTest_FirstOrDefault()
        {
            // Arrange
            repo = new MongoRepository<Restaurant>(new MongoDBContext());
        }

        [TestMethod]
        public void FirstOrDefault()
        {
            // Act
            var restaurant = repo.FirstOrDefault();

            // Assert
            restaurant.Should().NotBeNull();
            restaurant.Name.Should().Be(name);
        }

        [TestMethod]
        public void FirstOrDefault_Id()
        {
            // Act
            var restaurant = repo.FirstOrDefault(i => i.Id == id);

            // Assert
            restaurant.Should().NotBeNull();
            restaurant.Name.Should().Be(name);
        }

        [TestMethod]
        public void FirstOrDefault_Name()
        {
            // Act
            var restaurant = repo.FirstOrDefault(i => i.Name == name);

            // Assert
            restaurant.Should().NotBeNull();
            restaurant.Id.Should().Be(id);
        }

        [TestMethod]
        public void FirstOrDefault_StreetAddress()
        {
            // Act
            var restaurant = repo.FirstOrDefault(i => i.Address.Street.Equals(street));

            // Assert
            restaurant.Should().NotBeNull();
            System.Console.WriteLine(restaurant.Name);
        }

        [TestMethod]
        public void FirstOrDefault_GradeScore()
        {
            // Act
            var restaurant = repo.FirstOrDefault(i => i.Grades[0].Score > 10);

            // Assert
            restaurant.Should().NotBeNull();
            restaurant.Grades[0].Score.Should().BeGreaterThan(10);
            System.Console.WriteLine($"{restaurant.Name} - {restaurant.Grades[0].Score}");
        }

        /*=====================================================================================================================
         * ASYNC METHODS
         * ==================================================================================================================*/

        [TestMethod]
        public async Task FirstOrDefaultAsync_Id()
        {
            // Act
            var restaurant = await repo.FirstOrDefaultAsync(i => i.Id == id);

            // Assert
            restaurant.Should().NotBeNull();
            restaurant.Name.Should().Be(name);
        }

        [TestMethod]
        public async Task FirstOrDefaultAsync()
        {
            // Act
            var restaurant = await repo.FirstOrDefaultAsync();

            // Assert
            restaurant.Should().NotBeNull();
            restaurant.Name.Should().Be(name);
        }
    }
}
