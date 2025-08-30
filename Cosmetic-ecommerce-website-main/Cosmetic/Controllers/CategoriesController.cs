using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Cosmetic.Data;
using Cosmetic.Models;
using Microsoft.AspNetCore.Authorization;
using Cosmetic.Models.ViewModels;


namespace Cosmetic.Controllers
{
    [Authorize(Roles = "ADMIN")]
    public class CategoriesController : Controller
    {
        private readonly CosmeticContext _context;

        public CategoriesController(CosmeticContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var categories = await _context.Category
                                    .OrderBy(c => c.Id)
                                    .ToListAsync();


            return View(new CategoryManagementViewModel
            {
                Categories = categories,
                NewCategory = new CategoryCreateViewModel()
            });
        }


        public async Task<IActionResult> Details(long id)
        {

            var category = await _context.Category
                .FirstOrDefaultAsync(m => m.Id == id);
            if (category == null)
            {
                return NotFound();
            }

            return View(category);
        }

        public IActionResult Create()
        {
            return View();
        }


        [HttpPost]
        public async Task<IActionResult> CreateCategory(CategoryCreateViewModel newCategory)
        {

            if (ModelState.IsValid)
            {
                var checkCategory = await _context.Category.FirstOrDefaultAsync(c => c.Name == newCategory.Name);

                if (checkCategory != null)
                {
                    return Json(new { 
                        success = false,
                        message = "Category name already existed"
                    });
                }

                Category category = new Category
                {
                    Name = newCategory.Name,
                    Description = newCategory.Description,
                    Status = newCategory.Status,
                    CreateTime = DateTime.Now
                };
                _context.Category.Add(category);
                await _context.SaveChangesAsync();
                return Json(new
                {
                    success = true,
                    message = "Add category successfully!",
                    category
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
                message = "Failed to add new category",
                fieldErrors
            });
        }

        public async Task<IActionResult> EditCategoryPage(long id)
        {

            var category = await _context.Category.FindAsync(id);
            if (category == null)
            {
                return NotFound();
            }
            return View(new EditCategoryViewModel
            {
                Id = category.Id,
                Name = category.Name,
                Description = category.Description,
                Status = category.Status,
                CreateTime = category.CreateTime
            });
        }

        [HttpPut]
        public async Task<IActionResult> EditCategory( EditCategoryViewModel editCategory)
        {

            if (ModelState.IsValid)
            {
                Category category = await _context.Category.FindAsync(editCategory.Id);

                if (category == null) {
                    return Json(new
                    {
                        success = false,
                        message= "Category not found"
                    });
                }

                Category sameNameCategory = await _context.Category.FirstOrDefaultAsync(c => c.Name == editCategory.Name && c.Id != category.Id);
                if(sameNameCategory != null)
                {
                    return Json(new
                    {
                        success = false,
                        message = "This name already exist"
                    });
                }

                category.Status = editCategory.Status;
                category.Name = editCategory.Name;
                category.Description = editCategory.Description;
                await _context.SaveChangesAsync();
                return Json(new
                {
                    success = true,
                    message = "Update Successfully!"
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
                message = "Failed to add new category",
                fieldErrors
            });
        }

        [HttpPut]
        public async Task<IActionResult> ChangeStatusCategory([FromBody] ChangeCategoryStatus request)
        {
            long id = request.Id;
            bool isDelete = request.isDelete;
            Category category = await _context.Category.FindAsync(id);
            var text = isDelete ? "delete" : "restore";
            if (category == null)
            {

                return Json(new
                {
                    success = false,
                    message = $"Failed to {text} this category!!"

                });
            }
            category.Status = !isDelete;
            await _context.SaveChangesAsync();
            return Json(new
            {
                success = true,
                message = $"{text} successfully!!"
            });

        }


    }
}
