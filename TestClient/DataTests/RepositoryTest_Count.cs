using DataAccessLayer.Contexts;
using DataAccessLayer.Repositories.Abstract;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Models.Entities;
using static TestClient.Helpers.ExpectedValuesHelper;

namespace TestClient.DataTests
{
    [TestClass]
    public class RepositoryTest_Count
    {
        private MongoRepository<Restaurant> repo;        

        public RepositoryTest_Count()
        {
            repo = new MongoRepository<Restaurant>(new MongoDBContext());
        }

        [TestMethod]
        public void Count_Int()
        {
            var count = repo.Count();
            count.Should().Be(totalCount);
        }

        [TestMethod]
        public void Count_IntFiltered()
        {
            var count = repo.Count(i => i.Borough.Equals("Manhattan"));
            count.Should().Be(filteredManhattanCount);
        }

        [TestMethod]
        public void Count_Long()
        {
            var count = repo.LongCount();
            count.Should().Be(totalCount);
        }

        [TestMethod]
        public void Count_LongFiltered()
        {
            var count = repo.LongCount(i => i.Borough.Equals("Manhattan"));
            count.Should().Be(filteredManhattanCount);
        }
    }
}
