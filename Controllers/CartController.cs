using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using pet_food.Data;
using pet_food.Models;
using pet_food.Models.ViewModels;
using Microsoft.AspNetCore.Authorization;

namespace pet_food.Controllers
{
    [Authorize]
    public class CartController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<IdentityUser> _userManager;
        public CartController(ApplicationDbContext context, UserManager<IdentityUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }


        [HttpPost]
        public async Task<IActionResult> OrderConfirmed(string adress, string phone)
        {
            var uid =  _userManager.GetUserId(User);
            var carts =  _context.Carts.Where(f => f.UserId == uid &&
                                                 f.Status == "У кошику");
            foreach (var cart in carts)
            {
                cart.Status = "В обробці";
                _context.Entry(cart).State = EntityState.Modified;
            }
            _context.SaveChanges();
            List<string> model = new()
            {
                adress,
                phone
            };
            return View("OrderConfirmed",model);
        }

        public List<CartViewModel> CartGenerator()
        {
            List<CartViewModel> model1 = new();
            var uid = _userManager.GetUserId(User);
            var carts = _context.Carts.Where(x => x.UserId == uid);
            foreach (var cart in carts)
            {
                var feed = _context.Feeds.Find(cart.FeedId);
                if (feed != null)
                {
                    var feedtype = _context.FeedTypes.Find(feed.FeedTypeId);
                    var pet = _context.Pets.Find(feed.PetId);
                    if(feedtype!=null && pet != null)
                    {
                        model1.Add(new CartViewModel
                        {
                            Feed = new FeedPageViewModel
                            {
                                feed = feed,
                                FeedType = feedtype.Name,
                                Pet = pet.Name
                            },
                            Price =Math.Round(feed.Price * cart.Count,2),
                            Count = cart.Count,
                            Status = cart.Status,
                            Id=cart.Id
                        });
                    }
                }
            }
            List<CartViewModel> model = new();
            model.AddRange(model1.Where(m => m.Status == "У кошику"));
            model.AddRange(model1.Where(m => m.Status == "В обробці"));
            return model;
        }

        public async Task<IActionResult> OrderConfirmation()
        {
            return View(CartGenerator().Where(x => x.Status == "У кошику"));
        }

 
        public async Task<IActionResult> CartRemove(int cartId, int count)
        {
            var cart = await _context.Carts.FindAsync(cartId);
            if(cart != null)
            {
                if (count == 0)
                    _context.Carts.Remove(cart);
                else
                {
                    cart.Count = count;
                    _context.Entry(cart).State = EntityState.Modified;
                }
                _context.SaveChanges();
                return RedirectToAction("Index");
            }
            return NotFound();
        }
        
        public async Task<IActionResult> Index()
        {
            return View(CartGenerator());
        }

        [HttpPost]
        public async Task<IActionResult> CartAdd(int feedId, int count)
        {
            var feed = await _context.Feeds.FindAsync(feedId);
            if (feed != null)
            {
                var uid = _userManager.GetUserId(User);
                var carts = _context.Carts.ToList();
                bool modified = false;
                foreach (var cart in carts)
                {
                    if (feed.Id == cart.FeedId && uid == cart.UserId && cart.Status == "У кошику")
                    {
                        modified = true;
                        cart.Count += count;
                    }
                    _context.Entry(cart).State = EntityState.Modified;
                }
                if (!modified)
                {
                    _context.Carts.Add(
                    new Cart
                    {
                        Id = new Random().Next(),
                        Count = count,
                        UserId = _userManager.GetUserId(User),
                        FeedId = feed.Id,
                        Status = "У кошику"
                    });
                }
                _context.SaveChanges();
            }
            return RedirectToAction("Index", "Cart");
        }


        public async Task<IActionResult> Delete(int id)
        {
            var cart = await _context.Carts.FindAsync(id);
            if (cart != null)
            {
                _context.Carts.Remove(cart);
                _context.SaveChanges();
                return RedirectToAction("Index");
            }
            return NotFound();
        }
    
    }
}
