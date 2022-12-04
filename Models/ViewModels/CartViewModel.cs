namespace pet_food.Models.ViewModels
{
    public class CartViewModel
    {
        public double Price { get; set; }
        public int Count { get; set; }
        public string Status { get; set; }
        public int Id { get; set; }
        public FeedPageViewModel Feed { get; set; }
    }
}
