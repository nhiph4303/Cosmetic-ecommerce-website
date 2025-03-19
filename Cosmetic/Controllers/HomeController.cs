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
        private readonly UserManager<Customer> _userManager;
        private readonly SignInManager<Customer> _signInManager;
        private readonly CosmeticContext _context;

        public HomeController(ILogger<HomeController> logger, UserManager<Customer> userManager,
                              SignInManager<Customer> signInManager, CosmeticContext context)
        {
            _logger = logger;
            _userManager = userManager;
            _signInManager = signInManager;
            _context = context;
        }

        public IActionResult Index()
        {
            var categories = new List<string> { "Eyes", "Face", "Lips" };
            var products = _context.Product
                                   .Where(p => p.Category != null && categories.Contains(p.Category.Name))
                                   .ToList();

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

            return View(viewModel); // Trả về view với viewModel
        }
    

        public IActionResult ShoppingCart() => View();
        public IActionResult CheckOut() => View();
        public IActionResult AboutUs() => View();
        public IActionResult Compare() => View();
        public IActionResult Login() => View();

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

        // ViewModel cho trang Index (đã có sẵn)
        public class ProductViewModel
        {
            public List<Product> AllProducts { get; set; } = new List<Product>();
            public List<Product> ProductsUnder50 { get; set; } = new List<Product>();
        }

        
    }
}
