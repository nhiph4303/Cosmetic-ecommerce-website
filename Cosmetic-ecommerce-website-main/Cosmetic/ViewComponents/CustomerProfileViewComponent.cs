using Cosmetic.Data;
using Cosmetic.Models;
using Cosmetic.Models.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Cosmetic.ViewComponents
{
    public class CustomerProfileViewComponent : ViewComponent
    {
        private readonly CosmeticContext _context;
        private readonly UserManager<IdentityUser> _userManager;
        public CustomerProfileViewComponent(CosmeticContext context, UserManager<IdentityUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public async Task<IViewComponentResult> InvokeAsync(string mode = "Default")
        {
            var tempUser = HttpContext.User;

            if (!tempUser.Identity.IsAuthenticated)
            {
                return Content("User is not authenticated");
            }

            var user = await _userManager.GetUserAsync(tempUser);



            if (mode == "EditProfile")
            {
                Customer customerEditProfile = await _context.Customer.FirstOrDefaultAsync(eachCustomer => eachCustomer.UserId == user.Id);

                if (customerEditProfile == null)
                {
                    return Content("Customer not found");
                }
                
                return View(mode, new EditCustomerViewModel
                {
                    Name = customerEditProfile.Name,
                    Address = customerEditProfile.Address,
                    PhoneNumber = customerEditProfile.PhoneNumber,
                    DoB = customerEditProfile.DateOfBirth,
                    Email = user.Email,
                    Gender = customerEditProfile.Gender
                });
            }
            if (mode == "ChangePassword")
            {
                return View(mode, new ChangePasswordCustomerViewModel());
            }

            if(mode == "AddressShipping")
            {
                Customer customerAddressShipping = await _context.Customer.Include(c => c.AddressShippings).FirstOrDefaultAsync(eachCustomer => eachCustomer.UserId == user.Id);

                if (customerAddressShipping == null)
                {
                    return Content("Customer not found");
                }

                return View(mode, new AddressShippingViewModel
                {
                    ListAddressShipping = customerAddressShipping.AddressShippings
                });
            }
            Customer customer = await _context.Customer.Include(eachCustomer => eachCustomer.Rank).Include(c => c.User).FirstOrDefaultAsync(eachCustomer => eachCustomer.UserId == user.Id);
            return View(mode, customer);

        }
    }
}
