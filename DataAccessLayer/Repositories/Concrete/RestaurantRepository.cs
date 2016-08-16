using DataAccessLayer.Contexts;
using DataAccessLayer.Repositories.Abstract;
using Models.Entities;

namespace DataAccessLayer.Repositories.Concrete
{
    public class RestaurantRepository : MongoRepository<Restaurant>
    {
        public RestaurantRepository(MongoDBContext context) : base(context)
        { }
    }
}
