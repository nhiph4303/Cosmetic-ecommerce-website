using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Cosmetic.Data;
using Cosmetic.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;
using Cosmetic.Models.ViewModels;

namespace Cosmetic.Controllers
{
    [Authorize(Roles = "ADMIN")]
    public class ProductsController : Controller
    {
        private readonly CosmeticContext _context;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly int _pageSize = 10;
        public ProductsController(CosmeticContext context, UserManager<IdentityUser> userManager,
                             SignInManager<IdentityUser> signInManager,
                             RoleManager<IdentityRole> roleManager)
        {
            _context = context;
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
        }


        public async Task<IActionResult> Index(int page = 1)
        {
            var products = await _context.Product
                                    .Include(p => p.Category)
                                    .OrderBy(p => p.Id)
                                    .ToListAsync();

            var totalProducts = products.Count();
            var totalPages = (int)Math.Ceiling(totalProducts / (double)_pageSize);

            page = Math.Max(1, Math.Min(page, Math.Max(1, totalPages)));

            var result = products
                .OrderBy(p => p.Id)
                .Skip((page - 1) * _pageSize)
                .Take(_pageSize)
                .ToList();

            return View(new DashboardProductIndexViewModel
            {
                CurrentPage = page,
                TotalPages = totalPages,
                productList = result
            });
        }

        public async Task<IActionResult> ProductDetail(long id)
        {


            var product = await _context.Product.FindAsync(id);
            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }

        public async Task<IActionResult> CreateProduct()
        {
            var categories = await _context.Category.ToListAsync();

            return View(new ProductCreateViewModel
            {
                CategoryMenu = categories,
            });
        }

        [HttpPost]
        public async Task<IActionResult> CreateProduct(ProductCreateViewModel productCreateView, IFormFile ImageFile)
        {
            if (!ModelState.IsValid)
            {
                var fieldErrors = ModelState
                          .Where(x => x.Value.Errors.Count > 0)
                          .ToDictionary(
                          kvp => kvp.Key,
                          kvp => kvp.Value.Errors.First().ErrorMessage
                            );

                return Json(new
                {
                    success = false,
                    message = "Failed to create product",
                    fieldErrors
                });
            }

            var haveSameName = await _context.Product.FirstOrDefaultAsync(p => p.Name == productCreateView.Name);
            if (haveSameName != null)
            {
                return Json(new
                {
                    success = false,
                    message = "This name already exist"
                });
            }

            if (ImageFile == null || ImageFile.Length == 0)
            {
                var customFieldErrors = new Dictionary<string, string>();

                customFieldErrors["Image"] = "Image is required";
                return Json(new
                {
                    success = false,
                    message = "Failed to create product",
                    fieldErrors = customFieldErrors
                });
            }

            string imagePath = string.Empty;


            var uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/assets/images/products");
            Directory.CreateDirectory(uploadsFolder);

            var uniqueFileName = Guid.NewGuid().ToString() + Path.GetExtension(ImageFile.FileName);
            var filePath = Path.Combine(uploadsFolder, uniqueFileName);

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await ImageFile.CopyToAsync(stream);
            }

            imagePath = "/assets/images/products/" + uniqueFileName;

            Category category = await _context.Category.FirstOrDefaultAsync(c => c.Id == productCreateView.CategoryId);
            Product product = new Product
            {
                Category = category,
                CategoryId = category.Id,
                CreateTime = DateTime.Now,
                Description = productCreateView.Description,
                Discount = productCreateView.Discount,
                Name = productCreateView.Name,
                IsAvailable = productCreateView.IsAvailable,
                InStock = 0,
                ProductType = productCreateView.ProductType,
                CartItems = [],
                OrderDetails = [],
                ProductVariants = [],
                Image = imagePath
            };

            _context.Product.Add(product);
            await _context.SaveChangesAsync();

            return Json(new
            {
                success = true,
                message = "Create Successfully, Please create its product variants"
            });
        }

        public async Task<IActionResult> EditProduct(long id)
        {


            var product = await _context.Product
                .Include(p => p.Category)
                .Include(p => p.ProductVariants)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (product == null)
            {
                return NotFound();
            }

            var categories = await _context.Category.ToListAsync();

            return View(new ProductEditViewModel
            {
                Id = product.Id,
                ProductVariants = product.ProductVariants,
                CategoryId = product.CategoryId,
                CreateTime = product.CreateTime,
                Description = product.Description,
                Discount = product.Discount,
                Image = product.Image,
                InStock = product.InStock,
                IsAvailable = product.IsAvailable,
                Name = product.Name,
                ProductType = product.ProductType,
                CategoryMenu = categories
            });
        }


        [HttpPut]
        public async Task<IActionResult> EditProduct(ProductEditViewModel productEditView, IFormFile? ImageFile)
        {

            if (!ModelState.IsValid)
            {
                var fieldErrors = ModelState
                          .Where(x => x.Value.Errors.Count > 0)
                          .ToDictionary(
                          kvp => kvp.Key,
                          kvp => kvp.Value.Errors.First().ErrorMessage);
                return Json(new
                {
                    success = false,
                    message = "Failed to update product",
                    fieldErrors
                });
            }


            var id = productEditView.Id;
            var existingProduct = await _context.Product.FirstOrDefaultAsync(p => p.Id == id);
            if (existingProduct == null)
            {
                return NotFound();
            }

            var haveSameName = await _context.Product.FirstOrDefaultAsync(p => p.Id != id && p.Name == productEditView.Name);
            if (haveSameName != null)
            {
                return Json(new
                {
                    success = false,
                    message = "This name already exist"
                });
            }

            if (ImageFile != null && ImageFile.Length > 0)
            {
                var uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/assets/images/products");
                Directory.CreateDirectory(uploadsFolder);

                var uniqueFileName = Guid.NewGuid().ToString() + Path.GetExtension(ImageFile.FileName);
                var filePath = Path.Combine(uploadsFolder, uniqueFileName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await ImageFile.CopyToAsync(stream);
                }

                existingProduct.Image = "/assets/images/products/" + uniqueFileName;
            }

            existingProduct.IsAvailable = productEditView.IsAvailable;
            existingProduct.Name = productEditView.Name;
            existingProduct.Description = productEditView.Description;
            existingProduct.Discount = productEditView.Discount;
            existingProduct.CategoryId = productEditView.CategoryId;

            await _context.SaveChangesAsync();

            return Json(new
            {
                success = true,
                message = "Update Successfully!"
            });


        }


        [HttpPut]
        public async Task<IActionResult> ChangeProductStatus([FromBody] ChangeProductStatusViewModel changeProductStatus)
        {
            var id = changeProductStatus.Id;
            var isDelete = changeProductStatus.isDelete;
            Product product = await _context.Product.FindAsync(id);
            var text = isDelete ? "delete" : "restore";
            if (product == null)
            {

                return Json(new
                {
                    success = false,
                    message = $"Failed to {text}"
                });
            }

            product.IsAvailable = !isDelete;
            await _context.SaveChangesAsync();
            return Json(new
            {
                success = true,
                message = $"{text} successfully"
            });

        }
    }
}
