using Models.Attributes;
using Models.Entities;

namespace TestClient.TestModels
{
    [CollectionName("overriderestaurantname")]
    public class RestaurantWithAttributeOverride : Restaurant
    {
    }
}
