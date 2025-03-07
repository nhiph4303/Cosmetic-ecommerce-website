using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Cosmetic.Controllers
{
    public class AdminController : Controller
    {
        // GET: Admin
        public ActionResult Index()
        {
            return View();
        }

        // Manage Category
        public IActionResult Categories()
        {
            return View();
        }

        public IActionResult CategoryDetails(int id)
        {
            ViewData["CategoryId"] = id;
            return View();
        }

        // Manage Product
        public IActionResult Products()
        {
            return View();
        }

        public IActionResult ProductDetails(int id)
        {
            ViewData["ProductId"] = id;
            return View();
        }

        //Manage User
        public IActionResult Users()
        {
            return View();
        }

        public IActionResult UserDetails(int id)
        {
            ViewData["UserId"] = id;
            return View();
        }

        // Manage Logins
        public IActionResult Logins()
        {
            return View();
        }

        public IActionResult LoginDetails(int id)
        {
            ViewData["LoginId"] = id;
            return View();
        }

        // Manage Registers
        public IActionResult Registers()
        {
            return View();
        }

        public IActionResult RegisterDetails(int id)
        {
            ViewData["RegisterId"] = id;
            return View();
        }

        // Manage Orders
        public IActionResult Orders()
        {
            return View();
        }

        public IActionResult OrderDetails(int id)
        {
            ViewData["OrderId"] = id;
            return View();
        }

        // Manage News
        public IActionResult News()
        {
            return View();
        }

        public IActionResult NewsDetails(int id)
        {
            ViewData["NewsId"] = id;
            return View();
        }

        // GET: Admin/Category/Create
        public ActionResult CreateCategory()
        {
            return View();
        }

        // POST: Admin/Category/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateCategory(IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Categories));
            }
            catch
            {
                return View();
            }
        }

        // GET: Admin/Product/Create
        public ActionResult CreateProduct()
        {
            return View();
        }

        // POST: Admin/Product/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateProduct(IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Products));
            }
            catch
            {
                return View();
            }
        }

        // GET: Admin/User/Create
        public ActionResult CreateUser()
        {
            return View();
        }

        // POST: Admin/User/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateUser(IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Users));
            }
            catch
            {
                return View();
            }
        }
        // GET: Admin/Order/Create
        public ActionResult CreateOrder()
        {
            return View();
        }

        // POST: Admin/Order/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateOrder(IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Orders));
            }
            catch
            {
                return View();
            }
        }

        // GET: Admin/News/Create
        public ActionResult CreateNews()
        {
            return View();
        }

        // POST: Admin/News/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateNews(IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(News));
            }
            catch
            {
                return View();
            }
        }

        // GET: Admin/Category/Edit/5
        public ActionResult EditCategory(int id)
        {
            return View();
        }

        // POST: Admin/Category/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditCategory(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Categories));
            }
            catch
            {
                return View();
            }
        }

        // GET: Admin/Product/Edit/5
        public ActionResult EditProduct(int id)
        {
            return View();
        }

        // POST: Admin/Product/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditProduct(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Products));
            }
            catch
            {
                return View();
            }
        }

        // GET: Admin/User/Edit/5
        public ActionResult EditUser(int id)
        {
            return View();
        }

        // POST: Admin/User/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditUser(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Users));
            }
            catch
            {
                return View();
            }
        }

        // GET: Admin/Order/Edit/5
        public ActionResult EditOrder(int id)
        {
            return View();
        }

        // POST: Admin/Order/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditOrder(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Orders));
            }
            catch
            {
                return View();
            }
        }

        // GET: Admin/News/Edit/5
        public ActionResult EditNews(int id)
        {
            return View();
        }

        // POST: Admin/News/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditNews(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(News));
            }
            catch
            {
                return View();
            }
        }

        // GET: Admin/Category/Delete/5
        public ActionResult DeleteCategory(int id)
        {
            return View();
        }

        // POST: Admin/Category/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteCategory(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Categories));
            }
            catch
            {
                return View();
            }
        }

        // GET: Admin/Product/Delete/5
        public ActionResult DeleteProduct(int id)
        {
            return View();
        }

        // POST: Admin/Product/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteProduct(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Products));
            }
            catch
            {
                return View();
            }
        }

        // GET: Admin/User/Delete/5
        public ActionResult DeleteUser(int id)
        {
            return View();
        }

        // POST: Admin/User/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteUser(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Users));
            }
            catch
            {
                return View();
            }
        }

        // GET: Admin/Order/Delete/5
        public ActionResult DeleteOrder(int id)
        {
            return View();
        }

        // POST: Admin/Order/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteOrder(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Orders));
            }
            catch
            {
                return View();
            }
        }

        // GET: Admin/News/Delete/5
        public ActionResult DeleteNews(int id)
        {
            return View();
        }

        // POST: Admin/News/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteNews(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(News));
            }
            catch
            {
                return View();
            }
        }
    }
}
