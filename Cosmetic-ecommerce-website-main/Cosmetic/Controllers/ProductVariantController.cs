using Cosmetic.Data;
using Cosmetic.Models;
using Cosmetic.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Cosmetic.Controllers
{
    public class ProductVariantController : Controller
    {
        private readonly CosmeticContext _context;
        private readonly int _pageSize = 10;

        public ProductVariantController(CosmeticContext context)
        {
            _context = context;
        }
        public async Task<IActionResult> Index(int page = 1)
        {
            List<ProductVariant> productVariants = await _context.ProductVariant.Include(pv => pv.Product).ToListAsync();
            var totalProductVariants = productVariants.Count();
            var totalPages = (int)Math.Ceiling(totalProductVariants / (double)_pageSize);

            page = Math.Max(1, Math.Min(page, Math.Max(1, totalPages)));

            var result = productVariants
                .OrderBy(o => o.Id)
                .Skip((page - 1) * _pageSize)
                .Take(_pageSize)
                .ToList();
            return View(new ProductVariantIndexViewModel
            {
                ProductVariants = result,
                CurrentPage = page,
                TotalPages = totalPages,
            });
        }

        public async Task<IActionResult> EditProductVariant(long id)
        {
            ProductVariant productVariant = await _context.ProductVariant.Include(pv => pv.Product).FirstOrDefaultAsync(pv => pv.Id == id);

            if (productVariant == null)
            {
                return NotFound();
            }

            return View(new ProductVariantEditViewModel
            {
                Id = productVariant.Id,
                InStock = productVariant.InStock,
                Name = productVariant.Name,
                Price = productVariant.Price,
                Product = productVariant.Product
            });
        }

        [HttpPut]
        public async Task<IActionResult> EditProductVariant(ProductVariantEditViewModel productVariantEdit)
        {
            if (ModelState.IsValid)
            {
                ProductVariant productVariant = await _context.ProductVariant.Include(pv => pv.Product).FirstOrDefaultAsync(pv => pv.Id == productVariantEdit.Id);
                if (productVariant == null)
                {
                    return NotFound();
                }

                productVariant.Price = productVariantEdit.Price;
                productVariant.Product.InStock += (productVariantEdit.InStock - productVariant.InStock);

                productVariant.InStock = productVariantEdit.InStock;



                await _context.SaveChangesAsync();
                return Json(new
                {
                    success = true,
                    message = "Update successfully!"
                });
            }
            var fieldErrors = ModelState
                           .Where(x => x.Value.Errors.Count > 0)
                           .ToDictionary(
                           kvp => kvp.Key,
                           kvp => kvp.Value.Errors.First().ErrorMessage
           );
            return Json(new
            {
                success = false,
                message = "Failed to update product variant",
                fieldErrors
            });
        }

        public async Task<IActionResult> CreateProductVariant()
        {
            List<Product> products = await _context.Product.ToListAsync();

            return View(new ProductVariantCreateViewModel
            {
                Products = products
            });
        }

        [HttpPost]
        public async Task<IActionResult> CreateProductVariant(ProductVariantCreateViewModel model)
        {
            if (ModelState.IsValid)
            {
                Product product = await _context.Product.Include(p => p.ProductVariants).FirstOrDefaultAsync(p => p.Id == model.ProductId);

                List<ProductVariant> productVariants = product.ProductVariants;

                var customFieldErrors = new Dictionary<string, string>();
                if (product.ProductType.ToString() == "VolumeBased")
                {
                    model.Name = model.Name + "ml";
                }
                else if (product.ProductType.ToString() == "WeightBased")
                {
                    model.Name = model.Name + "g";
                }

                foreach (ProductVariant eachProductVariant in productVariants)
                {
                    if (eachProductVariant.Name == model.Name)
                    {
                        customFieldErrors["Name"] = "This name already exist";

                        return Json(new
                        {
                            success = false,
                            message = "Failed to create product variant",
                            fieldErrors = customFieldErrors
                        });
                    }
                }



                ProductVariant productVariant = new ProductVariant
                {
                    Name = model.Name,
                    InStock = model.InStock,
                    Product = product,
                    Price = model.Price,
                    ProductId = model.ProductId,
                };

                product.InStock += productVariant.InStock;

                _context.ProductVariant.Add(productVariant);
                await _context.SaveChangesAsync();

                return Json(new
                {
                    success = true,
                    message = "Create successfully!!"
                });
            }
            var fieldErrors = ModelState
                           .Where(x => x.Value.Errors.Count > 0)
                           .ToDictionary(
                           kvp => kvp.Key,
                           kvp => kvp.Value.Errors.First().ErrorMessage
           );
            return Json(new
            {
                success = false,
                message = "Failed to create product variant",
                fieldErrors
            });
        }
    }
}
