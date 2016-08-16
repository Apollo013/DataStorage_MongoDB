using DataAccessLayer.Contexts;
using DataAccessLayer.Repositories.Abstract;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Models.Entities;
using System;
using System.Linq;

namespace TestClient.DataTests
{
    [TestClass]
    public class RepositoryTest_Page
    {
        private MongoRepository<Restaurant> repo;

        public RepositoryTest_Page()
        {
            // Arrange
            repo = new MongoRepository<Restaurant>(new MongoDBContext());
        }

        [TestMethod]
        public void Page_NumSize_1_50()
        {
            // Arrange
            int pageNumber = 1;
            int pagesize = 50;

            // Act
            var restaurants = repo.Page(pageNumber, pagesize).ToList();

            // Assert
            restaurants.Count().Should().Be(pagesize);

            // Assure
            foreach (Restaurant res in restaurants)
            {
                Console.WriteLine(res.Name);
            }
        }

        [TestMethod]
        public void Page_NumSize_3_35()
        {
            // Arrange
            int pageNumber = 3;
            int pagesize = 35;

            // Act
            var restaurants = repo.Page(pageNumber, pagesize).ToList();

            // Assert
            restaurants.Count().Should().Be(pagesize);

            // Assure
            foreach (Restaurant res in restaurants)
            {
                Console.WriteLine(res.Name);
            }
        }


        /*===========================================================================================================
         * ORDERING - EXPRESSIONS
         * ========================================================================================================*/
        [TestMethod]
        public void Page_NumSize_1_50_ExpressionOrdering_Name_Asc()
        {
            // Arrange
            int pageNumber = 1;
            int pagesize = 50;

            // Act
            var restaurants = repo.Page(pageNumber, pagesize, (c => c.Name), false).ToList();

            // Assert
            restaurants.Count().Should().Be(pagesize);

            // Assure
            foreach (Restaurant res in restaurants)
            {
                Console.WriteLine($"{res.Name} - {res.Borough}");
            }
        }

        [TestMethod]
        public void Page_NumSize_1_50_ExpressionOrdering_Name_Desc()
        {
            // Arrange
            int pageNumber = 1;
            int pagesize = 50;

            // Act
            var restaurants = repo.Page(pageNumber, pagesize, (c => c.Name), true).ToList();

            // Assert
            restaurants.Count().Should().Be(pagesize);

            // Assure
            foreach (Restaurant res in restaurants)
            {
                Console.WriteLine($"{res.Name} - {res.Borough}");
            }
        }


        /*===========================================================================================================
         * ORDERING - DYNAMIC LINQ
         * ========================================================================================================*/
        [TestMethod]
        public void Page_NumSize_1_50_DynamicLinqOrdering()
        {
            // Arrange
            int pageNumber = 1;
            int pagesize = 50;

            // Act
            var restaurants = repo.Page(pageNumber, pagesize, "borough desc, name asc").ToList();

            // Assert
            restaurants.Count().Should().Be(pagesize);

            // Assure
            foreach (Restaurant res in restaurants)
            {
                Console.WriteLine($"{res.Name} - {res.Borough}");
            }
        }


        /*===========================================================================================================
         * FILTERING - EXPRESSIONS
         * ========================================================================================================*/
        [TestMethod]
        public void Page_NumSize_1_50_ExpressionFiltering() ///this 532
        {
            // Arrange
            int pageNumber = 1;
            int pagesize = 50;
            string borough = "Manhattan";

            // Act
            var restaurants = repo.Page(pageNumber, pagesize, (c => c.Name), true, (r => r.Borough.Equals(borough))).ToList();

            // Assert
            foreach (Restaurant res in restaurants)
            {
                res.Borough.Should().Be(borough);
                Console.WriteLine($"{res.Name} - {res.Borough}");
            }
        }


        /*===========================================================================================================
         * FILTERING - DYNAMIC LINQ
         * ========================================================================================================*/
        [TestMethod]
        public void Page_NumSize_1_50_DynamicLinqFiltering()
        {
            // Arrange
            int pageNumber = 1;
            int pagesize = 50;
            string borough = "Manhattan";

            // Act
            var restaurants = repo.Page(pageNumber, pagesize, "name desc", "borough.Equals(\"Manhattan\") AND name.Contains(\"Do\")").ToList();

            // Assert
            restaurants.Count().Should().Be(pagesize);

            foreach (Restaurant res in restaurants)
            {
                res.Borough.Should().Be(borough);
                res.Name.Should().Contain("Do");
                Console.WriteLine($"{res.Name} - {res.Borough}");
            }
        }

    }
}
