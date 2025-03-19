using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Cosmetic.Data;
using Shop.Models;
using Microsoft.AspNetCore.Identity;
using System.Diagnostics;
using System.Collections.Generic;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

namespace Cosmetic.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly UserManager<Customer> _userManager;
        private readonly SignInManager<Customer> _signInManager;
        private readonly CosmeticContext _context;

        public HomeController(ILogger<HomeController> logger, UserManager<Customer> userManager, SignInManager<Customer> signInManager, CosmeticContext context)
        {
            _logger = logger;
            _userManager = userManager;
            _signInManager = signInManager;
            _context = context;
        }

        public IActionResult Index()
        {
            // sản phẩm trong các danh mục Eyes, Face, Lips
            var categories = new List<string> { "Eyes", "Face", "Lips" };
            var products = _context.Product
                                   .Where(p => p.Category != null && categories.Contains(p.Category.Name))
                                   .ToList();

            //sản phẩm có giá dưới 50 đô
            var productsUnder50 = _context.Product
                                          .Where(p => p.Price < 50 && p.Category != null && categories.Contains(p.Category.Name))
                                          .ToList();

            Debug.WriteLine($"Total products returned (All): {products.Count}");
            Debug.WriteLine($"Total products returned (Under $50): {productsUnder50.Count}");

            var viewModel = new ProductViewModel
            {
                AllProducts = products,
                ProductsUnder50 = productsUnder50
            };

            return View(viewModel);
        }

        public async Task<IActionResult> Category(int? categoryId, string orderby, decimal? minPrice, decimal? maxPrice)
        {
            if (categoryId.HasValue)
            {
                var category = await _context.Category
                                              .FirstOrDefaultAsync(c => c.ID == categoryId.Value && c.Status == "active");

                if (category == null)
                {
                    return Content("Invalid category!");
                }
            }

            var products = _context.Product.AsQueryable();

            if (categoryId.HasValue)
            {
                products = products.Where(p => p.CategoryID == categoryId.Value);
            }

            if (minPrice.HasValue && maxPrice.HasValue)
            {
                products = products.Where(p => p.Price >= minPrice && p.Price <= maxPrice);
            }

            if (orderby == "alphabet-asc")
            {
                products = products.OrderBy(p => p.Name);
            }
            else if (orderby == "alphabet-desc")
            {
                products = products.OrderByDescending(p => p.Name);
            }

            var productList = await products.ToListAsync();

            ViewBag.SelectedCategoryId = categoryId;
            ViewBag.Categories = await _context.Category.Where(c => c.Status == "active").ToListAsync();
            ViewBag.OrderBy = orderby;
            ViewBag.MinPrice = minPrice;
            ViewBag.MaxPrice = maxPrice;

            return View(productList);
        }

        public IActionResult ProductDetail()
        {
            return View();
        }

        public IActionResult ShoppingCart()
        {
            return View();
        }

        public IActionResult CheckOut()
        {
            return View();
        }

        public IActionResult AboutUs()
        {
            return View();
        }

        public IActionResult Compare()
        {
            return View();
        }

        public IActionResult Login()
        {
            return View();
        }

        // Action Register (GET)
        public IActionResult Register()
        {
            return View();
        }

        // Action Register (POST)
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(Customer model)
        {
            if (ModelState.IsValid)
            {
                model.Status = "active";

                var user = new Customer
                {
                    UserName = model.Email,
                    Email = model.Email,
                    Name = model.Name,
                    Address = model.Address,
                    PhoneNumber = model.PhoneNumber,
                    Status = model.Status
                };

                var result = await _userManager.CreateAsync(user, model.Password);

                if (result.Succeeded)
                {
                    await _signInManager.SignInAsync(user, isPersistent: false);
                    TempData["Message"] = "Account created successfully!";
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    TempData["ErrorMessage"] = "An error occurred while creating the account.<br/>" +
                                               string.Join("<br/>", result.Errors.Select(e => e.Description));
                }
            }
            return View(model);
        }




        public class ProductViewModel
        {
            public List<Product> AllProducts { get; set; } = new List<Product>();
            public List<Product> ProductsUnder50 { get; set; } = new List<Product>();
        }
    }
}