using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace pet_food.Models
{
    public class Cart
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [ForeignKey("UserId")]
        public string UserId { get; set; }
        [ForeignKey("FeedId")]
        public int FeedId { get; set; }
        public int Count { get; set; }
        public string Status { get; set; }
    }
}
