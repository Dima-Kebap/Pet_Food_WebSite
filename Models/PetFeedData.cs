using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using pet_food.Data;

namespace pet_food.Models
{
    public class PetFeedData
    {
        public static async void Initialize(IServiceProvider serviceProvider, UserManager<IdentityUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            using (var context = new ApplicationDbContext(
                serviceProvider.GetRequiredService<DbContextOptions<ApplicationDbContext>>()))
            {
                //початковий користувач-адмін, та звичайний користувач, а також роль адміністратора
                if (!context.Users.Any())
                {
                    string adminEmail = "admin@gmail.com";
                    string userEmail = "user@gmail.com";
                    string password = "1234_Dd";
                    IdentityUser admin = new IdentityUser { Email = adminEmail, UserName = adminEmail, EmailConfirmed = true };
                    IdentityUser user = new IdentityUser { Email = userEmail, UserName = userEmail, EmailConfirmed = true };
                    var result1 = await userManager.CreateAsync(admin, password);
                    var result2 = await userManager.CreateAsync(user, password);
                    await roleManager.CreateAsync(new IdentityRole("admin"));
                    await userManager.AddToRoleAsync(admin, "admin");
                    context.SaveChanges();
                }
                //початкові види тварин
                if (!context.Pets.Any())
                {
                    context.Pets.AddRange(
                        new Pet
                        {
                            Name = "Котів",
                            Id = 1
                        },
                        new Pet
                        {
                            Name = "Собак",
                            Id = 2
                        },
                        new Pet
                        {
                            Name = "Птахів",
                            Id = 3
                        }
                    );
                    context.SaveChanges();
                }
                //початкові види кормів
                if (!context.FeedTypes.Any())
                {
                    context.FeedTypes.AddRange(
                        new FeedType
                        {
                            Name = "Сухий",
                            Id = 1
                        },
                        new FeedType
                        {
                            Name = "Консерва",
                            Id = 2
                        },
                        new FeedType
                        {
                            Name = "Ласощі",
                            Id = 3

                        }
                    );
                    context.SaveChanges();
                }
                //початкові товари корми
                if (!context.Feeds.Any())
                {
                    string description = "Lorem ipsum dolor sit amet, consectetur adipiscing elit. Curabitur laoreet pulvinar scelerisque. Vestibulum elementum nisi ut lorem maximus, a vulputate metus tristique. Mauris nec dolor sapien. Sed facilisis pretium diam, sed maximus mi pulvinar volutpat. Vestibulum ante ipsum primis in faucibus orci luctus et ultrices posuere cubilia curae; Pellentesque sed leo vitae neque vulputate aliquet. Pellentesque vitae lacinia lacus, in finibus neque. Nulla facilisi. Nulla blandit tempus massa ac aliquam. Aliquam finibus elementum libero, vitae ultricies tortor molestie id. Aliquam mattis leo id tempus varius. Quisque in ante erat. Phasellus cursus placerat ligula vel ullamcorper. Aliquam blandit convallis volutpat.";
                    context.Feeds.AddRange(
                        new Feed
                        {
                            Name = "JosiCat",
                            Price = 128,
                            Weight = 650,
                            Brand = "Josera",
                            Description = description,
                            Id = new Random().Next(),
                            FeedTypeId = 1,
                            PetId = 1,
                            Image = "cat_dry.jpg"
                        },
                        new Feed
                        {
                            Name = "Babycat",
                            Price = 86,
                            Weight = 195,
                            Brand = "Royal Canin",
                            Description = description,
                            Id = new Random().Next(),
                            FeedTypeId = 2,
                            PetId = 1,
                            Image = "cat_canned.jpg"
                        },
                        new Feed
                        {
                            Name = "GrasBits",
                            Price = 109.15,
                            Weight = 40,
                            Brand = "GimCat",
                            Description = description,
                            Id = new Random().Next(),
                            FeedTypeId = 3,
                            PetId = 1,
                            Image = "cat_dainty.png"
                        },
                        new Feed
                        {
                            Name = "Maxi",
                            Price = 884,
                            Weight = 4000,
                            Brand = "Royal Canin",
                            Description = description,
                            Id = new Random().Next(),
                            FeedTypeId = 1,
                            PetId = 2,
                            Image = "dog_dry.jpg"
                        },
                        new Feed
                        {
                            Name = "Mono Protein",
                            Price = 120.17,
                            Weight = 400,
                            Brand = "Brit",
                            Description = description,
                            Id = new Random().Next(),
                            FeedTypeId = 2,
                            PetId = 2,
                            Image = "dog_canned.png"
                        },
                        new Feed
                        {
                            Name = "Educ",
                            Price = 43,
                            Weight = 50,
                            Brand = "Royal Canin",
                            Description = description,
                            Id = new Random().Next(),
                            FeedTypeId = 3,
                            PetId = 2,
                            Image = "dog_dainty.jpg"
                        },
                        new Feed
                        {
                            Name = "Prestige",
                            Price = 78,
                            Weight = 1000,
                            Brand = "Versele-Laga",
                            Description = description,
                            Id = new Random().Next(),
                            FeedTypeId = 1,
                            PetId = 3,
                            Image = "bird_dry.jpg"
                        },
                        new Feed
                        {
                            Name = "Stix Tropical",
                            Price = 151.25,
                            Weight = 80,
                            Brand = "Padovan",
                            Description = description,
                            Id = new Random().Next(),
                            FeedTypeId = 3,
                            PetId = 3,
                            Image = "bird_dainty.jpg"
                        }

                    );
                    context.SaveChanges();
                }
            }
        }
    }
}