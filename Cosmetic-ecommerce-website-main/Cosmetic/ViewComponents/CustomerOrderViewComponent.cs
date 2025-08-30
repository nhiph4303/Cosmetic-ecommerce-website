using Cosmetic.Data;
using Cosmetic.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Cosmetic.ViewComponents
{
    public class CustomerOrderViewComponent : ViewComponent
    {
        private readonly CosmeticContext _context;
        private readonly UserManager<IdentityUser> _userManager;

        public CustomerOrderViewComponent(CosmeticContext context, UserManager<IdentityUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var tempUser = HttpContext.User;

            if (!tempUser.Identity.IsAuthenticated)
            {
                return Content("User is not authenticated");
            }

            var user = await _userManager.GetUserAsync(tempUser);

            Customer customer = await _context.Customer.Include(c => c.Orders).ThenInclude(o => o.OrderDetails).ThenInclude(od => od.Product).FirstOrDefaultAsync(c => c.UserId == user.Id);

            if (customer == null)
            {
                return Content("Customer not found");
            }
            
            return View(customer.Orders.OrderByDescending(o => o.Id));
        }
    }
}
