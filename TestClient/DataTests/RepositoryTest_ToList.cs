using DataAccessLayer.Contexts;
using DataAccessLayer.Repositories.Abstract;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Models.Entities;
using System.Linq;
using System.Threading.Tasks;
using static TestClient.Helpers.ExpectedValuesHelper;

namespace TestClient.DataTests
{
    [TestClass]
    public class RepositoryTest_ToList
    {

        private MongoRepository<Restaurant> repo;

        public RepositoryTest_ToList()
        {
            repo = new MongoRepository<Restaurant>(new MongoDBContext());
        }

        [TestMethod]
        public void ToList()
        {
            var restaurants = repo.ToList().ToList();
            restaurants.Count().Should().Be(totalCount);
        }

        [TestMethod]
        public async Task ToListAsync()
        {
            var restaurants = await repo.ToListAsync();
            restaurants.Count().Should().Be(totalCount);
        }
    }
}
