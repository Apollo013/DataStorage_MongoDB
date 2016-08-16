using Models.Entities;

namespace TestClient.TestModels
{
    public class RestaurantWithInheritedAttribute : Restaurant
    {
        public int Capacity { get; set; }
    }
}
