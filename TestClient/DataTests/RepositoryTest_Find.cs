using DataAccessLayer.Contexts;
using DataAccessLayer.Repositories.Abstract;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Models.Entities;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace TestClient.DataTests
{
    [TestClass]
    public class RepositoryTest_Find
    {
        private MongoRepository<Restaurant> repo;

        public RepositoryTest_Find()
        {
            // Arrange
            repo = new MongoRepository<Restaurant>(new MongoDBContext());
        }

        [TestMethod]
        public void Find_Borough_ExpressionFiltering()
        {
            // Act
            var restaurants = repo.Find(i => i.Borough.Equals("Manhattan")).ToList();

            // Assert
            restaurants.Count.Should().Be(10259);

            for (int i = 0; i < 10; i++)
            {
                restaurants[i].Borough.Should().Be("Manhattan");
                Console.WriteLine($"{restaurants[i].Name} - {restaurants[i].Borough}");
            }
        }

        [TestMethod]
        public void Find_Borough_ExpressionFiltering_ExpressionOrdering_Name_Desc()
        {
            // Act
            var restaurants = repo.Find(i => i.Borough.Equals("Manhattan"), (i => i.Name), true).ToList();

            // Assert
            restaurants.Count.Should().Be(10259);

            for (int i = 0; i < 10; i++)
            {
                restaurants[i].Borough.Should().Be("Manhattan");
                Console.WriteLine($"{restaurants[i].Name} - {restaurants[i].Borough}");
            }
        }

        [TestMethod]
        public void Find_Borough_ExpressionFiltering_ExpressionOrdering_Name_Asc()
        {
            // Act
            var restaurants = repo.Find(i => i.Borough.Equals("Manhattan"), (i => i.Name), false).ToList();

            // Assert
            restaurants.Count.Should().Be(10259);

            for (int i = 0; i < 10; i++)
            {
                restaurants[i].Borough.Should().Be("Manhattan");
                Console.WriteLine($"{restaurants[i].Name} - {restaurants[i].Borough}");
            }
        }


        [TestMethod]
        public void Find_Borough_DynamicLinqFiltering()
        {
            // Arrange
            var filter = "borough.Equals(\"Manhattan\") AND name.Contains(\"Do\")";

            // Act
            var restaurants = repo.Find(filter).ToList();

            // Assert
            foreach (Restaurant res in restaurants)
            {
                res.Borough.Should().Be("Manhattan");
                res.Name.Should().Contain("Do");
                Console.WriteLine($"{res.Name} - {res.Borough}");
            }
        }

        [TestMethod]
        public void Find_Name_DynamicLinqFiltering()
        {
            // Arrange
            var filter = "name.Equals(\"Fairfield Inn Suites Penn Station\")";

            // Act
            var restaurants = repo.Find(filter).ToList();

            // Assert
            foreach (Restaurant res in restaurants)
            {
                res.Name.Should().Be("Fairfield Inn Suites Penn Station");
                Console.WriteLine($"{res.Id} - {res.Name} - {res.Borough}");
            }
        }

        [TestMethod]
        public void Find_Borough_DynamicLinqFiltering_DynamicLinqOrdering_Name_Desc()
        {
            // Arrange
            var filter = "borough.Equals(\"Manhattan\") AND name.Contains(\"Do\")";
            var order = "name desc";

            // Act
            var restaurants = repo.Find(filter, order).ToList();

            // Assert
            foreach (Restaurant res in restaurants)
            {
                res.Borough.Should().Be("Manhattan");
                res.Name.Should().Contain("Do");
                Console.WriteLine($"{res.Name} - {res.Borough}");
            }
        }

        [TestMethod]
        public void Find_Borough_DynamicLinqFiltering_DynamicLinqOrdering_Name_Asc()
        {
            // Arrange
            var filter = "borough.Equals(\"Manhattan\") AND name.Contains(\"Do\")";
            var order = "name";

            // Act
            var restaurants = repo.Find(filter, order).ToList();

            // Assert
            foreach (Restaurant res in restaurants)
            {
                res.Borough.Should().Be("Manhattan");
                res.Name.Should().Contain("Do");
                Console.WriteLine($"{res.Name} - {res.Borough}");
            }
        }

        /*=====================================================================================================================
         * ASYNC METHODS
         * ==================================================================================================================*/

        [TestMethod]
        public async Task FindAsync_Borough_ExpressionFiltering()
        {
            // Act
            var restaurants = await repo.FindAsync(i => i.Borough.Equals("Manhattan"));

            // Assert
            restaurants.AsQueryable().Count().Should().Be(10259);

            var count = 0;
            foreach (var res in restaurants)
            {
                res.Borough.Should().Be("Manhattan");
                Console.WriteLine($"{res.Name} - {res.Borough}");
                count++;
                if (count == 10) return;
            }
        }

        [TestMethod]
        public async Task FindAsync_Borough_ExpressionFiltering_ExpressionOrdering_Name_Desc()
        {
            // Act
            var restaurants = await repo.FindAsync(i => i.Borough.Equals("Manhattan"), (i => i.Name), true);

            // Assert
            restaurants.AsQueryable().Count().Should().Be(10259);

            var count = 0;
            foreach (var res in restaurants)
            {
                res.Borough.Should().Be("Manhattan");
                Console.WriteLine($"{res.Name} - {res.Borough}");
                count++;
                if (count == 10) return;
            }
        }

        [TestMethod]
        public async Task FindAsync_Borough_ExpressionFiltering_ExpressionOrdering_Name_Asc()
        {
            // Act
            var restaurants = await repo.FindAsync(i => i.Borough.Equals("Manhattan"), (i => i.Name), false);

            // Assert
            restaurants.AsQueryable().Count().Should().Be(10259);

            var count = 0;
            foreach (var res in restaurants)
            {
                res.Borough.Should().Be("Manhattan");
                Console.WriteLine($"{res.Name} - {res.Borough}");
                count++;
                if (count == 10) return;
            }
        }


        [TestMethod]
        public async Task FindAsync_Borough_DynamicLinqFiltering()
        {
            // Arrange
            var filter = "borough.Equals(\"Manhattan\") AND name.Contains(\"Do\")";

            // Act
            var restaurants = await repo.FindAsync(filter);

            // Assert
            foreach (Restaurant res in restaurants)
            {
                res.Borough.Should().Be("Manhattan");
                res.Name.Should().Contain("Do");
                Console.WriteLine($"{res.Name} - {res.Borough}");
            }
        }

        [TestMethod]
        public async Task FindAsync_Name_DynamicLinqFiltering()
        {
            // Arrange
            var filter = "name.Equals(\"Fairfield Inn Suites Penn Station\")";

            // Act
            var restaurants = await repo.FindAsync(filter);

            // Assert
            foreach (var res in restaurants)
            {
                res.Name.Should().Be("Fairfield Inn Suites Penn Station");
                Console.WriteLine($"{res.Id} - {res.Name} - {res.Borough}");
            }
        }

        [TestMethod]
        public async Task FindAsync_Borough_DynamicLinqFiltering_DynamicLinqOrdering_Name_Desc()
        {
            // Arrange
            var filter = "borough.Equals(\"Manhattan\") AND name.Contains(\"Do\")";
            var order = "name desc";

            // Act
            var restaurants = await repo.FindAsync(filter, order);

            // Assert
            foreach (var res in restaurants)
            {
                res.Borough.Should().Be("Manhattan");
                res.Name.Should().Contain("Do");
                Console.WriteLine($"{res.Name} - {res.Borough}");
            }
        }

        [TestMethod]
        public async Task FindAsync_Borough_DynamicLinqFiltering_DynamicLinqOrdering_Name_Asc()
        {
            // Arrange
            var filter = "borough.Equals(\"Manhattan\") AND name.Contains(\"Do\")";
            var order = "name";

            // Act
            var restaurants = await repo.FindAsync(filter, order);

            // Assert
            foreach (var res in restaurants)
            {
                res.Borough.Should().Be("Manhattan");
                res.Name.Should().Contain("Do");
                Console.WriteLine($"{res.Name} - {res.Borough}");
            }
        }
    }
}
