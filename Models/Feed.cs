using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace pet_food.Models
{
    public class Feed
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string Name { get; set; } 
        public string Brand { get; set; } 
        public int Weight { get; set; } 
        public double Price { get; set; }
        public string Description { get; set; }

        [ForeignKey("FeedTypeId")]
        public int FeedTypeId { get; set; }
        [ForeignKey("PetId")]
        public int PetId { get; set; }
        public string Image { get; set; }
    }
}
