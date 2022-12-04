
namespace pet_food.Models.ViewModels
{
    public class Filter
    {
        public List<string> Brands { get; set; }
        public int Weight { get; set; }
        public double minPrice { get; set; }
        public double maxPrice { get; set; }
        public List<int> FeedTypesId { get; set; }
        public List<int> PetsId { get; set; }

        public Filter()
        {
            PetsId = new List<int>();
            FeedTypesId = new List<int>();
            Brands = new List<string>();
        }
    }

}
