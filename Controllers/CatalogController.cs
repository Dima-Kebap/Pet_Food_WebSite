using Microsoft.AspNetCore.Mvc;
using pet_food.Data;
using pet_food.Models;
using pet_food.Models.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;



namespace pet_food.Controllers
{
    public class CatalogController : Controller
    {
        private readonly ApplicationDbContext _context;
        public CatalogController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Filter(Filter filter)
        {
            var FilteredFeeds = _context.Feeds.Where(f => f.Price >= filter.minPrice &&
                                     f.Price <= filter.maxPrice &&
                                     f.Weight <= filter.Weight);
            if (filter.Brands.Any())           
                FilteredFeeds = FilteredFeeds.Where(f => filter.Brands.Contains(f.Brand));
            
            if (filter.FeedTypesId.Any())           
                FilteredFeeds = FilteredFeeds.Where(f => filter.FeedTypesId.Contains(f.FeedTypeId));
            
            if (filter.PetsId.Any())           
                FilteredFeeds = FilteredFeeds.Where(f => filter.PetsId.Contains(f.PetId));
            
            List<FeedPageViewModel> allFilteredFeeds = new();
            foreach (var feed in FilteredFeeds)
            {
                var feedtype = await _context.FeedTypes.FindAsync(feed.FeedTypeId);
                var pet = await _context.Pets.FindAsync(feed.PetId);
                if (feedtype != null && pet != null)
                {
                    allFilteredFeeds.Add(new FeedPageViewModel
                    {
                        feed = feed,
                        FeedType = feedtype.Name,
                        Pet = pet.Name
                    });
                }

            }
            var allfeeds = _context.Feeds.ToList();
            CatalogViewModel model = new()
            {
                AllFeeds = allfeeds,
                FilteredFeeds = allFilteredFeeds
            };
            return View("Catalog",model);
        }

        public async Task<IActionResult> Catalog()
        {
            List<FeedPageViewModel> allFilteredFeeds = new();
            foreach (var feed in _context.Feeds)
            {
                var feedtype = await _context.FeedTypes.FindAsync(feed.FeedTypeId);
                var pet = await _context.Pets.FindAsync(feed.PetId);
                if (feedtype != null && pet != null)
                    allFilteredFeeds.Add(new FeedPageViewModel
                    {
                        feed = feed,
                        FeedType = feedtype.Name,
                        Pet = pet.Name
                    });
            }
            CatalogViewModel model = new()
            {
                AllFeeds = _context.Feeds.ToList(),
                FilteredFeeds = allFilteredFeeds
            };

            return View(model);
        }

        [Authorize(Roles = "admin")]
        [HttpGet("create")]
        public async Task<IActionResult> Create()
        {            
            var allFeedTypes = _context.FeedTypes.ToList();
            var allPets = _context.Pets.ToList();
            FeedCreateViewModel model = new()
            {
                AllFeedTypes = allFeedTypes,
                AllPets = allPets
            };
            return View(model);
        }

        [Authorize(Roles = "admin")]
        [HttpPost("create")]
        public IActionResult Create(Feed feed, IFormFile image)
        {
            feed.Id = new Random().Next();
            string fullPath = "wwwroot/images/feeds/" + image.FileName;
            feed.Image = image.FileName;
            using (var fileStream = new FileStream(fullPath, FileMode.Create))
            {
                image.CopyToAsync(fileStream);
            }
            _context.Feeds.Add(feed);
            _context.SaveChanges();
            return RedirectToAction("Catalog");
        }


        [Authorize(Roles = "admin")]
        public async Task<IActionResult> Delete(int id)
        {
            var feed = await _context.Feeds.FindAsync(id);
            if (feed != null)
            {
                _context.Feeds.Remove(feed);
                _context.SaveChanges();
            }
            return RedirectToAction("Catalog");
        }
 
        public async Task<IActionResult> FeedPage(int id)
        {
            var feed = await _context.Feeds.FindAsync(id);
            if (feed != null)
            {
                var feedtype = await _context.FeedTypes.FindAsync(feed.FeedTypeId);
                var pet = await _context.Pets.FindAsync(feed.PetId);
                if (feedtype != null && pet != null)
                {
                    FeedPageViewModel model = new()
                    {
                        feed = feed,
                        FeedType = feedtype.Name,
                        Pet = pet.Name
                    };
                    return View(model);
                }
            }
            return NotFound();
        }

        [Authorize(Roles = "admin")]
        [HttpGet("Edit")]
        public async Task<IActionResult> Edit(int id)
        {
            var feed = await _context.Feeds.FindAsync(id);
            if (feed != null)
            {
                var allFeedTypes = _context.FeedTypes.ToList();
                var allPets = _context.Pets.ToList();
                FeedCreateViewModel types_pets = new()
                {
                    AllFeedTypes = allFeedTypes,
                    AllPets = allPets
                };
                FeedEditViewModel model = new()
                {
                    All_Types_Pets = types_pets,
                    Feed = feed
                };
                return View(model);
            }
            return NotFound();
        }


        [Authorize(Roles = "admin")]
        [HttpPost("Edit")]
        public async Task<IActionResult> Edit(Feed edit, IFormFile image)
        {
            var feed = await _context.Feeds.FindAsync(edit.Id);
            if (feed != null)
            {
                if (image != null)
                {
                    string fullPath = "wwwroot/images/feeds/" + image.FileName;
                    feed.Image = image.FileName;
                    using var fileStream = new FileStream(fullPath, FileMode.Create);
                        await image.CopyToAsync(fileStream);
                }

                feed.Name = edit.Name;
                feed.Description = edit.Description;
                feed.Price = edit.Price;
                feed.Weight = edit.Weight;
                feed.Brand = edit.Brand;
                feed.FeedTypeId = edit.FeedTypeId;
                feed.PetId = edit.PetId;

                _context.Entry(feed).State = EntityState.Modified;
                _context.SaveChanges();
                return RedirectToAction("FeedPage", new { id = edit.Id });
            }
            return NotFound();
        }
    }

}

