using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using pet_food.Models;
namespace pet_food.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public DbSet<Feed> Feeds { get; set; }
        public DbSet<Cart> Carts { get; set; }
        public DbSet<FeedType> FeedTypes { get; set; }
        public DbSet<Pet> Pets { get; set; }
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
    }
}