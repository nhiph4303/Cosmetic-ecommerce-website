using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Cosmetic.Data;
using Shop.Models;

namespace Cosmetic.Controllers
{
    public class ProductsController : Controller
    {
        private readonly CosmeticContext _context;

        public ProductsController(CosmeticContext context)
        {
            _context = context;
        }

        // GET: Products
        //public async Task<IActionResult> Index()
        //{
        //    return View(await _context.Product.ToListAsync());
        //}
       
        public IActionResult Index()
        {
            var products = _context.Product
                                    .Include(p => p.Category)  
                                    .OrderByDescending(p => p.CreateTime)  
                                    .ToList();
       
            return View(products);
        }




        // GET: Products/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var product = await _context.Product
                .FirstOrDefaultAsync(m => m.ID == id);
            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }

        // GET: Products/Create
        //public IActionResult Create()
        //{
        //    return View();
        //}
        public IActionResult Create()
        {
            var categories = _context.Category.ToList();

            ViewBag.Categories = categories;

            return View();
        }


        // POST: Products/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> Create([Bind("Name,Price,Image,InStock,Status")] Product product)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        _context.Add(product);
        //        await _context.SaveChangesAsync();
        //        return RedirectToAction(nameof(Index));
        //    }

        //    if (string.IsNullOrEmpty(product.Image))
        //    {
        //        product.Image = "~/assets/images/dashboard/upload.svg";  
        //    }

        //    return View(product);
        //}
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Name,Description,Price,InStock,Status,CategoryID")] Product product, IFormFile? ImageFile)
        {
            if (!ModelState.IsValid)
            {
                TempData["Error"] = "Validation failed. Please check your inputs.";
                return View(product);
            }

            try
            {
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

                    product.Image = "/assets/images/products/" + uniqueFileName;
                }
                else
                {
                    product.Image = "/assets/images/dashboard/upload.svg";
                }

                product.CreateTime = DateTime.Now;
                _context.Add(product);
                await _context.SaveChangesAsync();

                TempData["Success"] = "Product has been created!";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                TempData["Error"] = "Error creating product: " + ex.Message;
                return View(product);
            }
        }










        // GET: Products/Edit/5
        //public async Task<IActionResult> Edit(int? id)
        //{
        //    if (id == null)
        //    {
        //        return NotFound();
        //    }

        //    var product = await _context.Product.FindAsync(id);
        //    if (product == null)
        //    {
        //        return NotFound();
        //    }

        //    // Lấy danh sách danh mục và truyền vào ViewBag
        //    var categories = await _context.Category.ToListAsync();
        //    ViewBag.Categories = categories;

        //    return View(product);
        //}
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var product = await _context.Product
                .Include(p => p.Category)  
                .FirstOrDefaultAsync(m => m.ID == id);

            if (product == null)
            {
                return NotFound();
            }

            ViewBag.Categories = new SelectList(await _context.Category.ToListAsync(), "ID", "Name", product.CategoryID);

            return View(product);
        }







        // POST: Products/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> Edit(int id, [Bind("ID,Name,Price,Image,InStock,Status")] Product product)
        //{
        //    if (id != product.ID)
        //    {
        //        return NotFound();
        //    }

        //    if (ModelState.IsValid)
        //    {
        //        try
        //        {
        //            _context.Update(product);
        //            await _context.SaveChangesAsync();
        //        }
        //        catch (DbUpdateConcurrencyException)
        //        {
        //            if (!ProductExists(product.ID))
        //            {
        //                return NotFound();
        //            }
        //            else
        //            {
        //                throw;
        //            }
        //        }
        //        return RedirectToAction(nameof(Index));
        //    }
        //    return View(product);
        //}
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ID,Name,Description,Price,InStock,Status,CategoryID,Image")] Product product, IFormFile? ImageFile)
        {
            if (id != product.ID)
            {
                return NotFound();
            }

            ModelState.Remove("ImageFile"); 

            if (!ModelState.IsValid)
            {
                TempData["Error"] = "Validation failed. Please check your inputs.";
                ViewBag.Categories = new SelectList(await _context.Category.ToListAsync(), "ID", "Name", product.CategoryID);
                return View(product);
            }

            try
            {
                var existingProduct = await _context.Product.FindAsync(id);
                if (existingProduct == null)
                {
                    return NotFound();
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

                existingProduct.Name = product.Name;
                existingProduct.Description = product.Description;
                existingProduct.Price = product.Price;
                existingProduct.InStock = product.InStock;
                existingProduct.Status = product.Status;
                existingProduct.CategoryID = product.CategoryID;

                _context.Update(existingProduct);
                await _context.SaveChangesAsync();

                TempData["Success"] = "Product has been updated!";
                return RedirectToAction(nameof(Index));
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!_context.Product.Any(e => e.ID == product.ID))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
        }












        // GET: Products/Delete/5
        //public async Task<IActionResult> Delete(int? id)
        //{
        //    if (id == null)
        //    {
        //        return NotFound();
        //    }

        //    var product = await _context.Product
        //        .FirstOrDefaultAsync(m => m.ID == id);
        //    if (product == null)
        //    {
        //        return NotFound();
        //    }

        //    return View(product);
        //}
        public async Task<IActionResult> Delete(int? id)
        {
            var product = await _context.Product.FindAsync(id);
            if (product != null)
            {
                _context.Product.Remove(product);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));

        }

        // POST: Products/Delete/5
        //[HttpPost, ActionName("Delete")]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> DeleteConfirmed(int id)
        //{
        //    var product = await _context.Product.FindAsync(id);
        //    if (product != null)
        //    {
        //        _context.Product.Remove(product);
        //    }

        //    await _context.SaveChangesAsync();
        //    return RedirectToAction(nameof(Index));
        //}
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var product = await _context.Product.FindAsync(id);

            if (product == null)
            {
                return Json(new { success = false, message = "Product not found!" });
            }

            _context.Product.Remove(product);
            await _context.SaveChangesAsync();

            return Json(new { success = true, id = id });
        }


        private bool ProductExists(int id)
        {
            return _context.Product.Any(e => e.ID == id);
        }
    }
}
