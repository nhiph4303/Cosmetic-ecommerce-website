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
            return View();
        }

        public async Task<IActionResult> Category(int? categoryId)
        {
            if (categoryId.HasValue)
            {
                var categoryExists = await _context.Category.AnyAsync(c => c.ID == categoryId);
                if (!categoryExists)
                {
                    return Content("Danh mục không hợp lệ!"); // Bạn có thể chuyển hướng về trang danh mục chính
                }
            }

            var categories = await _context.Category
                .Where(c => c.Status == "active")
                .OrderBy(c => c.Name)
                .ToListAsync();

            var products = await _context.Product
                .Where(p => !categoryId.HasValue || p.CategoryID == categoryId)
                .OrderByDescending(p => p.CreateTime)
                .ToListAsync();

            ViewBag.SelectedCategoryId = categoryId;
            ViewBag.Categories = categories;

            return View(products);
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
}
