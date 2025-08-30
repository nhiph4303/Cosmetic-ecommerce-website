using Cosmetic.Data;
using Cosmetic.Models.ViewModels;
using Cosmetic.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Http.HttpResults;

namespace Cosmetic.ViewComponents
{
    public class CustomerAddressShippingViewComponent : ViewComponent
    {
        private readonly CosmeticContext _context;
        private readonly UserManager<IdentityUser> _userManager;
        public CustomerAddressShippingViewComponent(CosmeticContext context, UserManager<IdentityUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public async Task<IViewComponentResult> InvokeAsync(string mode = "Default", long id = 0)
        {
            var tempUser = HttpContext.User;

            if (!tempUser.Identity.IsAuthenticated)
            {
                return Content("User is not authenticated");
            }

            var user = await _userManager.GetUserAsync(tempUser);

            Customer customer = await _context.Customer.Include(eachCustomer => eachCustomer.AddressShippings).FirstOrDefaultAsync(eachCustomer => eachCustomer.UserId == user.Id);

            if (customer == null)
            {
                return Content("Customer not found");
            }
            if (mode == "AddAddressShipping")
            {
               
                return View(mode, new AddressShippingViewModel() );
            }

            if (mode == "EditAddressShipping")
            {
                AddressShipping addressShipping = await _context.AddressShipping.FindAsync(id);

                return View(mode, new AddressShippingViewModel
                {
                    Name = addressShipping.Name,
                    District = addressShipping.District,
                    IsDefaultAddress = addressShipping.IsDefaultAddress,
                    PhoneNumber = addressShipping.PhoneNumber,
                    Province = addressShipping.Province,
                    SpecificPlace = addressShipping.SpecificPlace,
                    Ward = addressShipping.Ward,
                    Id = id
                });
            }


            return View(mode,new AddressShippingViewModel
            {
                ListAddressShipping = customer.AddressShippings
            });

        }
    }
}
