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
    public class CategoriesController : Controller
    {
        private readonly CosmeticContext _context;

        public CategoriesController(CosmeticContext context)
        {
            _context = context;
        }

        // GET: Categories
        //public async Task<IActionResult> Index()
        //{
        //    var categories = await _context.Category.ToListAsync();
        //    return View(categories);
        //}
        public IActionResult Index()
        {
            var categories = _context.Category
                                    .OrderByDescending(c => c.CreateTime) 
                                    .ToList();

            return View(categories); 
        }


        // GET: Categories/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var category = await _context.Category
                .FirstOrDefaultAsync(m => m.ID == id);
            if (category == null)
            {
                return NotFound();
            }

            return View(category);
        }

        // GET: Categories/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Categories/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ID,Name,Description,Status")] Category category)
        {
            if (ModelState.IsValid)
            {
                //_context.Add(category);
                //await _context.SaveChangesAsync();
                //return RedirectToAction(nameof(Index));
                category.CreateTime = DateTime.Now;
                _context.Category.Add(category); 
                _context.SaveChanges(); 
                return RedirectToAction("Index");
            }
            return View(category);
        }

        // GET: Categories/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var category = await _context.Category.FindAsync(id);
            if (category == null)
            {
                return NotFound();
            }
            return View(category);
        }

        // POST: Categories/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        private bool CategoryExists(int id)
        {
            return _context.Category.Any(e => e.ID == id);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ID,Name,Description,Status")] Category category)
        {
            if (id != category.ID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    category.CreateTime = DateTime.Now;

                    _context.Update(category);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CategoryExists(category.ID))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(category);
        }


        // GET: Categories/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            var category = await _context.Category.FindAsync(id);
            if (category != null)
            {
                _context.Category.Remove(category);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));

        }


        // POST: Categories/Delete/5
        //[HttpPost, ActionName("Delete")]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> DeleteConfirmed(int id)
        //{
        //    var category = await _context.Category.FindAsync(id);
        //    if (category != null)
        //    {
        //        _context.Category.Remove(category);
        //    }

        //    await _context.SaveChangesAsync();
        //    return RedirectToAction(nameof(Index));
        //}

        //private bool CategoryExists(int id)
        //{
        //    return _context.Category.Any(e => e.ID == id);
        //}
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var category = await _context.Category.FindAsync(id);

            if (category == null)
            {
                return Json(new { success = false, message = "Category not found!" });
            }

            _context.Category.Remove(category);
            await _context.SaveChangesAsync();

            return Json(new { success = true, id = id });
        }

    }
}
