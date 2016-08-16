using DataAccessLayer.Contexts;
using DataAccessLayer.Repositories.Abstract;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Models.Entities;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;

namespace TestClient.DataTests
{
    [TestClass]
    public class RepositoryTest_Persistence
    {
        private MongoRepository<Restaurant> repo;

        public RepositoryTest_Persistence()
        {
            // Arrange
            repo = new MongoRepository<Restaurant>(new MongoDBContext());
        }

        /*======================================================================================================
         * ADD
         * ===================================================================================================*/
        [TestMethod]
        public void AddOne()
        {
            var res = new Restaurant()
            {
                Name = "Test Restaurant",
                Cuisine = "God Awful Stuff",
                RestaurantId = "11111111",
                Borough = "DA TOWN",
                Address = new Address()
                {
                    Building = "341",
                    Street = "DA STREET",
                    Zipcode = "502633",
                    Coord = new double[] { -73.856077, 40.848445 }
                },
                Grades = new List<RestaurantGrade>()
                {
                    new RestaurantGrade() {Date = DateTime.Now.AddMonths(-1), Grade = "B", Score = 16 },
                    new RestaurantGrade() {Date = DateTime.Now, Grade = "A", Score = 25 }
                }
            };

            var resp = repo.InsertOne(res);
            resp.Id.Should().NotBeNullOrWhiteSpace();
            Console.WriteLine(resp.Id);
        }

        /*======================================================================================================
         * UPDATE
         * ===================================================================================================*/
        [TestMethod]
        public void Update()
        {
            var res = repo.GetById("57aca55e6a9b17bcb5aac0a6");
            res.Should().NotBeNull();

            var ack = repo.Update<string>((i => i.Id == res.Id), (i => i.Name), "Some Forgotten Name");
            ack.Should().Be(true);
        }

        [TestMethod]
        public void UpdateManyFiltered()
        {
            var res = repo.Find((i => i.Name == "")).ToList();
            Console.WriteLine("Before: " + res.Count);

            if (res.Count > 0)
            {
                var ack = repo.Update<string>((i => i.Name == ""), (i => i.Name), "Some Forgotten Name");
                ack.Should().Be(true);
            }

            res = repo.Find((i => i.Name == "")).ToList();
            Console.WriteLine("After: " + res.Count);
            res.Count.Should().Be(0);
        }

        [TestMethod]
        public void UpdateManyIdUpdateDefinition()
        {
            // Arrange
            var builder = Builders<Restaurant>.Update;

            List<UpdateDefinition<Restaurant>> updates = new List<UpdateDefinition<Restaurant>>();

            updates.Add(builder.Set("borough", "Unknown Borough 1"));
            updates.Add(builder.Set("cuisine", "Unknown Cuisine 2"));

            // Act
            var ack = repo.Update("57aca55e6a9b17bcb5aac0a6", updates.ToArray());

            // Assert
            ack.Should().Be(true);
            var res = repo.GetById("57aca55e6a9b17bcb5aac0a6");
            res.Borough.Should().Be("Unknown Borough 1");
            res.Cuisine.Should().Be("Unknown Cuisine 2");
        }

        [TestMethod]
        public void UpdateManyFilterExpressionUpdateDefinition()
        {
            // Arrange
            var builder = Builders<Restaurant>.Update;

            List<UpdateDefinition<Restaurant>> updates = new List<UpdateDefinition<Restaurant>>();

            updates.Add(builder.Set("borough", "Unknown Borough 3"));
            updates.Add(builder.Set("cuisine", "Unknown Cuisine 3"));

            // Act
            var ack = repo.Update((i => i.Name.Equals("Some Forgotten Name")), updates.ToArray());

            // Assert
            ack.Should().Be(true);

            var res = repo.Find((i => i.Name.Equals("Some Forgotten Name"))).ToList();
            foreach (var r in res)
            {
                r.Borough.Should().Be("Unknown Borough 3");
                r.Cuisine.Should().Be("Unknown Cuisine 3");
            }
        }

        [TestMethod]
        public void UpdateManyFilterAndUpdateDefinition()
        {
            // Arrange
            var filterBuilder = Builders<Restaurant>.Filter;
            var filterDef = filterBuilder.Eq("name", "Some Forgotten Name");

            var updateBuilder = Builders<Restaurant>.Update;
            List<UpdateDefinition<Restaurant>> updateDefinitions = new List<UpdateDefinition<Restaurant>>();
            updateDefinitions.Add(updateBuilder.Set("borough", "Unknown Borough 4"));
            updateDefinitions.Add(updateBuilder.Set("cuisine", "Unknown Cuisine 5"));


            // Act
            var ack = repo.Update(filterDef, updateDefinitions.ToArray());

            // Assert
            ack.Should().Be(true);

            var res = repo.Find((i => i.Name.Equals("Some Forgotten Name"))).ToList();
            foreach (var r in res)
            {
                r.Borough.Should().Be("Unknown Borough 4");
                r.Cuisine.Should().Be("Unknown Cuisine 5");
            }
        }

        /*======================================================================================================
         * DELETE
         * ===================================================================================================*/
        [TestMethod]
        public void Replace()
        {
            // Arrange

            // Get a document that already exists
            var currentDoc = repo.Last();
            Console.WriteLine(currentDoc.Id);

            // Now completely change it
            var res = new Restaurant()
            {
                Id = currentDoc.Id,
                Name = "Replacement Restaurant",
                Cuisine = "God Awful Stuff",
                RestaurantId = "11111111",
                Borough = "DA TOWN",
                Address = new Address()
                {
                    Building = "341",
                    Street = "DA STREET",
                    Zipcode = "502633",
                    Coord = new double[] { -73.856077, 40.848445 }
                },
                Grades = new List<RestaurantGrade>()
                {
                    new RestaurantGrade() {Date = DateTime.Now.AddMonths(-1), Grade = "B", Score = 16 },
                    new RestaurantGrade() {Date = DateTime.Now, Grade = "A", Score = 25 }
                }
            };

            // Act
            var ack = repo.ReplaceOne(res);
            var newDoc = repo.GetById(currentDoc.Id);

            // Assert
            ack.Should().Be(true);
            newDoc.Name.Should().Be("Replacement Restaurant");
        }

        /*======================================================================================================
         * DELETE
         * ===================================================================================================*/
        [TestMethod]
        public void Delete()
        {
            /*
            // Act
            var countBefore = repo.Count();
            repo.Delete("57b2e37c81180920b4438930");
            var countAfter = repo.Count();

            // Assert
            countAfter.Should().Be(countBefore - 1);
            Console.WriteLine(countAfter);
            */
        }

    }
}
