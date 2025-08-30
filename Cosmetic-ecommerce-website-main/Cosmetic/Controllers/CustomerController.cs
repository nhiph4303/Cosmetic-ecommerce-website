using System.Threading.Tasks;
using Cosmetic.Data;
using Cosmetic.Helper;
using Cosmetic.Models;
using Cosmetic.Models.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Cosmetic.Controllers
{
    public class CustomerController : Controller
    {
        private readonly CosmeticContext _context;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public CustomerController(CosmeticContext context, UserManager<IdentityUser> userManager,
                             SignInManager<IdentityUser> signInManager,
                             RoleManager<IdentityRole> roleManager)
        {
            _context = context; _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
        }


        [Authorize(Roles = "CUSTOMER")]
        public async Task<IActionResult> Index(string section,string mode, long id = -1)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound();
            }
         
            Customer customer = await _context.Customer.Include(c => c.User).FirstOrDefaultAsync(c => c.UserId == user.Id);
            if (customer == null)
            {
                return NotFound();
            }
            ViewBag.Section = section;
            ViewBag.Mode = mode;
            ViewBag.IdEdit = id;
            return View(customer);
        }

        [HttpPut]
        [Authorize(Roles ="CUSTOMER,ADMIN")]
        public async Task<IActionResult> ChangeInforCustomer(EditCustomerViewModel editCustomerViewModel)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.GetUserAsync(User);
                if (user == null)
                {
                    return NotFound();
                }
                Customer customer = await _context.Customer.FirstOrDefaultAsync(c => c.UserId == user.Id);
                if (customer == null)
                {
                    return NotFound();
                }
                customer.PhoneNumber = editCustomerViewModel.PhoneNumber;
                customer.Address = editCustomerViewModel.Address;
                customer.DateOfBirth = editCustomerViewModel.DoB;
                customer.Gender = editCustomerViewModel.Gender;
                customer.Name = editCustomerViewModel.Name;

                await _context.SaveChangesAsync();
                return Json(new
                {
                    success = true,
                    message = "Edit Successfully!!",
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
                fieldErrors
            });

        }

        [HttpPut]
        [Authorize(Roles = "CUSTOMER,ADMIN")]
        public async Task<IActionResult> ChangePasswordCustomer(ChangePasswordCustomerViewModel changePasswordCustomerViewModel)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.GetUserAsync(User);
                if (user == null)
                {
                    return NotFound();
                }
                Customer customer = await _context.Customer.FirstOrDefaultAsync(c => c.UserId == user.Id);
                if (customer == null)
                {
                    return NotFound();
                }
                var customFieldErrors = new Dictionary<string, string>();
                if (changePasswordCustomerViewModel.NewPassword != changePasswordCustomerViewModel.ConfirmPassword)
                {
                    customFieldErrors[nameof(changePasswordCustomerViewModel.ConfirmPassword)] = "New password not match";
                    return Json(new
                    {
                        success = false,
                        fieldErrors = customFieldErrors
                    });
                }

                var result = await _userManager.ChangePasswordAsync(user, changePasswordCustomerViewModel.Password, changePasswordCustomerViewModel.NewPassword);

                if (!result.Succeeded)
                {
                    foreach (var error in result.Errors)
                    {
                        if (error.Code == "PasswordMismatch")
                        {
                            customFieldErrors[nameof(changePasswordCustomerViewModel.Password)] = "Current password is incorrect.";
                        }
                    }
                    if (customFieldErrors.Any())
                    {
                        return Json(new
                        {
                            success = false,
                            fieldErrors = customFieldErrors
                        });
                    }
                }

               


                await _signInManager.RefreshSignInAsync(user);
                return Json(new
                {
                    success = true,
                    message = "Change Password Successfully!!",
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
                fieldErrors
            });

        }

        [HttpPost]
        [Authorize(Roles = "CUSTOMER")]
        public async Task<IActionResult> AddNewAddressShipping(AddressShippingViewModel addressShippingViewModel)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.GetUserAsync(User);
                if (user == null)
                {
                    return NotFound();
                }
                Customer customer = await _context.Customer.Include(c => c.AddressShippings).FirstOrDefaultAsync(c => c.UserId == user.Id);
                if (customer == null)
                {
                    return NotFound();
                }
                AddressShipping addressShipping = new AddressShipping(addressShippingViewModel.Name, addressShippingViewModel.Province, addressShippingViewModel.District, addressShippingViewModel.Ward, addressShippingViewModel.PhoneNumber, addressShippingViewModel.IsDefaultAddress, addressShippingViewModel.SpecificPlace, customer.Id, customer);
                bool isDefault = addressShippingViewModel.IsDefaultAddress;
                if (isDefault)
                {
                    AddressShipping defaultAddressShipping = customer.AddressShippings.FirstOrDefault(addShip => addShip.IsDefaultAddress);
                    if (defaultAddressShipping != null)
                    {
                        defaultAddressShipping.IsDefaultAddress = false;
                    }
                }
                customer.AddressShippings.Add(addressShipping);
                await _context.SaveChangesAsync();
                return Json(new
                {
                    success = true,
                    message = "Add New Shipping Address Successfully!!",
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
                fieldErrors,
            });
        }

        [HttpPut]
        [Authorize(Roles = "CUSTOMER")]
        public async Task<IActionResult> EditAddressShipping(AddressShippingViewModel addressShippingViewModel)
        {
            if (ModelState.IsValid)
            {
                var isDefault = addressShippingViewModel.IsDefaultAddress;
                var user = await _userManager.GetUserAsync(User);
                if (user == null)
                {
                    return NotFound();
                }
                Customer customer = await _context.Customer.Include(c => c.AddressShippings).FirstOrDefaultAsync(c => c.UserId == user.Id);
                if (customer == null)
                {
                    return NotFound();
                }
                AddressShipping addressShipping = customer.AddressShippings.Find(addShip => addShip.Id == addressShippingViewModel.Id);
                addressShipping.PhoneNumber = addressShippingViewModel.PhoneNumber;
                addressShipping.Ward = addressShippingViewModel.Ward;
                addressShipping.District = addressShippingViewModel.District;
                addressShipping.Name = addressShippingViewModel.Name;
                addressShipping.Province = addressShippingViewModel.Province;
                addressShipping.SpecificPlace = addressShippingViewModel.SpecificPlace;
                if (isDefault)
                {
                    AddressShipping defaultAddress = customer.AddressShippings.Find(addShip => addShip.IsDefaultAddress);
                    if (defaultAddress != null)
                    {
                        defaultAddress.IsDefaultAddress = false;
                    }
                }
                addressShipping.IsDefaultAddress = isDefault;
                await _context.SaveChangesAsync();
                return Json(new
                {
                    success = true,
                    message = "Edit Shipping Address Successfully!!",
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
                fieldErrors,
            });
        }

        [HttpPost]
        [Authorize(Roles = "CUSTOMER")]
        public async Task<IActionResult> DeleteAddressShipping(long id)
        {
            if (ModelState.IsValid)
            {
                var newDefaultId = 0L;
                AddressShipping addressShipping = await _context.AddressShipping.FirstOrDefaultAsync(addShip => addShip.Id == id);
                if (addressShipping.IsDefaultAddress)
                {
                    var user = await _userManager.GetUserAsync(User);
                    if (user == null)
                    {
                        return NotFound();
                    }
                    Customer customer = await _context.Customer.Include(c => c.AddressShippings).FirstOrDefaultAsync(c => c.UserId == user.Id);
                    if (customer == null)
                    {
                        return NotFound();
                    }

                    var remainingAddresses = customer.AddressShippings
                        .Where(a => a.Id != addressShipping.Id)
                        .ToList();

                    if (remainingAddresses.Count > 0)
                    {
                        remainingAddresses[0].IsDefaultAddress = true;
                        newDefaultId = remainingAddresses[0].Id;
                    }
                }


                _context.AddressShipping.Remove(addressShipping);
                await _context.SaveChangesAsync();

                return Json(new
                {
                    success = true,
                    message = "Delete Shipping Address Successfully!!",
                    newDefaultId
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
                fieldErrors
            });
        }


        [HttpPost]
        [Authorize(Roles = "CUSTOMER")]
        public async Task<IActionResult> SetDefaultAddressShipping(long id)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound();
            }
            Customer customer = await _context.Customer.Include(c => c.AddressShippings).FirstOrDefaultAsync(c => c.UserId == user.Id);
            if (customer == null)
            {
                return NotFound();
            }
            List<AddressShipping> listAddressShippings = customer.AddressShippings;
            AddressShipping defaultAddress = listAddressShippings.Find(addShip => addShip.IsDefaultAddress);
            if (defaultAddress != null)
            {
                defaultAddress.IsDefaultAddress = false;
            }
            AddressShipping nextDefaultAddress = listAddressShippings.Find(addShip => addShip.Id == id);
            nextDefaultAddress.IsDefaultAddress = true;
            await _context.SaveChangesAsync();
            return Json(new
            {
                success = true,
                message = "Set Default Successfully!!"
            });
        }
    }
}
