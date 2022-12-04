namespace pet_food.Models.ViewModels
{
    public class CatalogViewModel
    {
        public List<Feed> AllFeeds { get; set; }
        public List<FeedPageViewModel> FilteredFeeds { get; set; }

        public CatalogViewModel()
        {
            AllFeeds = new List<Feed>();
            FilteredFeeds = new List<FeedPageViewModel>();
        }

    }
}
