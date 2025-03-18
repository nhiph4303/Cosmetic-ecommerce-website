using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Cosmetic.Data;
using Shop.Models;

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
            // Lấy tất cả sản phẩm trong các danh mục Eyes, Face, Lips
            var categories = new List<string> { "Eyes", "Face", "Lips" };
            var products = _context.Product
                                   .Where(p => p.Category != null && categories.Contains(p.Category.Name))
                                   .ToList();

            // Lấy sản phẩm có giá dưới 50 đô
            var productsUnder50 = _context.Product
                                          .Where(p => p.Price < 50 && p.Category != null && categories.Contains(p.Category.Name))
                                          .ToList();

            Console.WriteLine($"Total products returned (All): {products.Count}");
            Console.WriteLine($"Total products returned (Under $50): {productsUnder50.Count}");

            // Trả về View và truyền dữ liệu cả hai: products và productsUnder50
            var viewModel = new ProductViewModel
            {
                AllProducts = products,
                ProductsUnder50 = productsUnder50
            };

            return View(viewModel);  // Trả về viewModel chứa cả 2 danh sách
        }



        // Phương thức Category
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

            // theo id
            var products = _context.Product.AsQueryable();

            if (categoryId.HasValue)
            {
                products = products.Where(p => p.CategoryID == categoryId.Value); // id
            }

            // sort giá
            if (minPrice.HasValue && maxPrice.HasValue)
            {
                products = products.Where(p => p.Price >= minPrice && p.Price <= maxPrice);
            }

            // sort tên
            if (orderby == "alphabet-asc")
            {
                products = products.OrderBy(p => p.Name);
            }
            else if (orderby == "alphabet-desc")
            {
                products = products.OrderByDescending(p => p.Name);
            }

            var productList = await products.ToListAsync();

            // truyền data
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

        public IActionResult Register()
        {
            return View();
        }


        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View("Error");
        }


    }

    // Inside the HomeController class
    public class ProductViewModel
    {
        public List<Product> AllProducts { get; set; } = new List<Product>();
        public List<Product> ProductsUnder50 { get; set; } = new List<Product>();
    }
}
