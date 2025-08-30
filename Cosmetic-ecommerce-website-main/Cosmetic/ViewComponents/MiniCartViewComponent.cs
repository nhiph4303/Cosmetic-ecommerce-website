using Cosmetic.Data;
using Cosmetic.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity.Pages.Manage;

namespace Cosmetic.ViewComponents
{
    public class MiniCartViewComponent : ViewComponent
    {
        private readonly CosmeticContext _context;
        private readonly UserManager<IdentityUser> _userManager;
        public MiniCartViewComponent(CosmeticContext context, UserManager<IdentityUser> userManager)
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

            Customer customer = await _context.Customer.Include(eachCustomer => eachCustomer.Cart).ThenInclude(eachCart => eachCart.CartItems).FirstOrDefaultAsync(eachCustomer => eachCustomer.UserId == user.Id);
            if (customer == null) {
                return Content("Customer not found");
            }
            return View(customer.Cart);

        }
    }
}
