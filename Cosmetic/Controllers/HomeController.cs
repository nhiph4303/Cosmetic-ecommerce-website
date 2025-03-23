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
using System;

namespace Cosmetic.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly CosmeticContext _context;

        public HomeController(ILogger<HomeController> logger, CosmeticContext context)
        {
            _logger = logger;
            _context = context;
        }

        public IActionResult Index()
        {
            var categories = new List<string> { "Eyes", "Face", "Lips" };

            var products = _context.Product
                                   .Where(p => (p.Category != null && categories.Contains(p.Category.Name) && p.Status != "out of stock"))
                                   .ToList();

            var productsUnder50 = _context.Product
                                          .Where(p => p.Price < 50 && p.Category != null && categories.Contains(p.Category.Name) && p.Status != "out of stock")
                                          .ToList();

            Debug.WriteLine($"Total products returned (All): {products.Count}");
            Debug.WriteLine($"Total products returned (Under $50): {productsUnder50.Count}");

            var viewModel = new ProductViewModel
            {
                AllProducts = products,
                ProductsUnder50 = productsUnder50
            };

            ViewBag.WelcomeMessage = TempData["Message"];
            ViewBag.ErrorMessage = TempData["ErrorMessage"];

            return View(viewModel);
        }


        public IActionResult Register() => View();
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(Customer model)
        {
            if (ModelState.IsValid)
            {
                var existingCustomer = await _context.Customer
                                                     .FirstOrDefaultAsync(c => c.Email == model.Email);
                if (existingCustomer != null)
                {
                    TempData["ErrorMessage"] = "Email is already in use.";
                    return RedirectToAction("Register");
                }

                model.Status = "active";

                _context.Customer.Add(model);
                await _context.SaveChangesAsync();

                TempData["Message"] = "Registration successful! You can now log in.";

                return RedirectToAction("Login");
            }

            return View(model);
        }
        public IActionResult Login()
        {
            var userEmail = HttpContext.Session.GetString("UserEmail");

            if (userEmail != null)
            {
                
                TempData["Message"] = "You are already logged in.";
            }

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Login(string email, string password)
        {
            var customer = _context.Customer.FirstOrDefault(c => c.Email == email);

            if (customer == null)
            {
                TempData["ErrorMessage"] = "Account does not exist. Please check your email and try again.";
                return View(); 
            }

            if (customer.Password == password)
            {
                HttpContext.Session.SetString("UserEmail", customer.Email);

                TempData["Message"] = $"Hello, {customer.Email}";

                return RedirectToAction("Index"); 
            }
            else
            {
                TempData["ErrorMessage"] = "Invalid credentials. Please try again.";
                return View(); 
            }
        }

        [HttpPost]
        public IActionResult Logout()
        {
            HttpContext.Session.Remove("UserEmail");
            HttpContext.Session.Clear();

            TempData["Message"] = "You have logged out successfully.";

            return RedirectToAction("Login");
        }


        [HttpGet]
        [Route("home/category")]
        public async Task<IActionResult> Category(int? categoryId, string orderby, decimal? minPrice, decimal? maxPrice, string searchQuery)
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

            products = products.Where(p => p.Status != "out of stock");

            if (!string.IsNullOrEmpty(searchQuery))
            {
                products = products.Where(p => p.Name.Contains(searchQuery) || p.Category.Name.Contains(searchQuery));
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
            ViewBag.SearchQuery = searchQuery;

            return View(productList);
        }

        public class ProductDetailViewModel
        {
            public Product Product { get; set; }
            public List<Product> RelatedProducts { get; set; }
        }

        public async Task<IActionResult> ProductDetail(int id)
        {
            var product = await _context.Product
                .Include(p => p.Category)
                .FirstOrDefaultAsync(p => p.ID == id);

            if (product == null)
            {
                return NotFound();
            }

            var relatedProducts = await _context.Product
                .Where(p => p.ID != id)
                .Take(8)
                .ToListAsync();

            var viewModel = new ProductDetailViewModel
            {
                Product = product,
                RelatedProducts = relatedProducts
            };

            return View(viewModel);
        }
        public IActionResult ShoppingCart() => View();
        public IActionResult CheckOut() => View();
        public IActionResult AboutUs() => View();

        [HttpGet]
        [Route("home/compare/{productId}")]
        public IActionResult Compare(int productId)
        {
            var selectedProduct = _context.Product
                                          .Include(p => p.Category)
                                          .FirstOrDefault(p => p.ID == productId);

            if (selectedProduct == null)
            {
                return NotFound();
            }

            var relatedProducts = _context.Product
                                          .Where(p => p.CategoryID == selectedProduct.CategoryID && p.ID != selectedProduct.ID)
                                          .OrderBy(r => Guid.NewGuid())  
                                          .Take(3) 
                                          .ToList();

            ViewData["SelectedProduct"] = selectedProduct;
            ViewData["RelatedProducts"] = relatedProducts;

            return View();
        }



        public class ProductViewModel
        {
            public List<Product> AllProducts { get; set; } = new List<Product>();
            public List<Product> ProductsUnder50 { get; set; } = new List<Product>();
        }

    }
}
