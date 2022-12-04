namespace pet_food.Models.ViewModels
{
    public class FeedCreateViewModel
    {
        public List<FeedType> AllFeedTypes { get; set; }
        public List<Pet> AllPets { get; set; }
        public FeedCreateViewModel()
        {
            AllFeedTypes = new List<FeedType>();
            AllPets = new List<Pet>();
        }
    }
}
