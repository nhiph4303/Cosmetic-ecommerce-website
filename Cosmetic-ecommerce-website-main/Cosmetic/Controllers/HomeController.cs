
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Cosmetic.Data;
using Microsoft.AspNetCore.Identity;
using Cosmetic.Models;
using Cosmetic.Helper;
using Cosmetic.Models.ViewModels;
using Cosmetic.DTO.Cart;
using Cosmetic.DTO.CartItem;
using Cosmetic.DTO.Product;
using Cosmetic.DTO.ProductVariant;
using Microsoft.AspNetCore.Authorization;

namespace Cosmetic.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly CosmeticContext _context;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        private readonly PasswordService passwordService = new PasswordService();

        public HomeController(ILogger<HomeController> logger, CosmeticContext context, UserManager<IdentityUser> userManager,
                             SignInManager<IdentityUser> signInManager,
                             RoleManager<IdentityRole> roleManager)
        {
            _logger = logger;
            _context = context;
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
        }


        // Done
        [AllowAnonymous]
        public async Task<IActionResult> Index()
        {
            List<string> categoryMenu = new List<string> { "Eyes", "Face", "Lips" };

            List<Product> products = await _context.Product
                                   .Where(eachProduct => (categoryMenu.Contains(eachProduct.Category.Name) && eachProduct.IsAvailable && eachProduct.ProductVariants.Any(productVariant => productVariant.InStock > 0)))
                                   .Include(eachProduct => eachProduct.ProductVariants.Where(eachProductVariant => eachProductVariant.InStock > 0))
                                   .ToListAsync();
            Product product = products.First(p => p.Discount > 0);


            List<Product> productsHasPriceUnder50 = await _context.Product
                                                                    .Where(eachProduct =>
                                                                            categoryMenu.Contains(eachProduct.Category.Name) &&
                                                                            eachProduct.IsAvailable &&
                                                                            eachProduct.ProductVariants.Any(productVariant => productVariant.InStock > 0 && productVariant.Price <= 50)
                                                                          )
                                                                    .Include(eachProduct => eachProduct.ProductVariants.Where(eachProductVariant =>
                                                                             eachProductVariant.InStock > 0 && eachProductVariant.Price <= 50
                                                                            ))
                                                    .ToListAsync();

            ViewData["SpecialProduct"] = product;
            var user = await _userManager.GetUserAsync(User);
            ViewBag.UserEmail = user == null ? null : user.Email;
            return View(new ProductIndexViewModel
            {
                products = products,
                productsHasPriceUnder50 = productsHasPriceUnder50
            });
        }

        // Done
        [AllowAnonymous]
        public IActionResult Register() => View();

        // Done
        [AllowAnonymous]
        [HttpPost]
        public async Task<IActionResult> Register(RegisterViewModel registerViewModel)
        {
            if (ModelState.IsValid)
            {

                var email = registerViewModel.Email;
                var name = registerViewModel.Name;
                var password = registerViewModel.Password;
                var confirmPassword = registerViewModel.ConfirmPassword;
                var DoB = registerViewModel.DoB;
                var gender = registerViewModel.Gender;
                var address = registerViewModel.Address;
                var phoneNumber = registerViewModel.PhoneNumber;
                var checkUser = await _userManager.FindByEmailAsync(email);

                if (checkUser != null)
                {
                    ModelState.AddModelError("", "Email is already in use");
                    return View(registerViewModel);
                }



                if (password != confirmPassword)
                {
                    ModelState.AddModelError("", "Password does not match");
                    return View(registerViewModel);
                }

                if (DateTime.Today <= DoB)
                {
                    ModelState.AddModelError("", "Invalid Date");
                    return View(registerViewModel);
                }

                var user = new IdentityUser { Email = email, UserName = email };
                var result = await _userManager.CreateAsync(user, password);

                if (result.Succeeded)
                {
                    await _userManager.AddToRoleAsync(user, "CUSTOMER");
                    Rank rank = await _context.Rank.FirstOrDefaultAsync(eachRank => eachRank.Id == 1);
                    Customer customer = new Customer
                    {
                        Address = address,
                        DateOfBirth = DoB,
                        Gender = gender,
                        IsActive = true,
                        Name = name,
                        PhoneNumber = phoneNumber,
                        User = user,
                        UserId = user.Id,
                        Rank = rank,
                        RankId = rank.Id,
                        StartDate = DateTime.Now,

                    };
                    _context.Customer.Add(customer);
                    await _context.SaveChangesAsync();

                    Cart cart = new Cart(customer.Id, customer);
                    await _context.Cart.AddAsync(cart);
                    await _context.SaveChangesAsync();

                    customer.Cart = cart;
                    TempData["RegisterSuccessMessage"] = "Registration successful! You can now log in.";

                    return RedirectToAction("Register");
                }

                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }

                return View(registerViewModel);

            }

            return View(registerViewModel);
        }

        // Done
        [AllowAnonymous]
        public async Task<IActionResult> Login(string? returnURL)
        {
            await _signInManager.SignOutAsync();
            ViewBag.ReturnURL = returnURL;
            return View();
        }


        // Done
        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> Login(LoginViewModel loginViewModel, string? returnURL)
        {

            if (ModelState.IsValid)
            {
                var email = loginViewModel.Email;
                var password = loginViewModel.Password;
                var rememberMe = loginViewModel.RememberMe;

                var user = await _userManager.FindByEmailAsync(email);

                if (user == null)
                {
                    ModelState.AddModelError("", "Email does not exist");
                    return View(loginViewModel);
                }

                var result = await _signInManager.PasswordSignInAsync(user, password, rememberMe, false);

                if (!result.Succeeded)
                {
                    ModelState.AddModelError("", "Password is not correct");
                    return View(loginViewModel);
                }

                TempData["LoginSuccess"] = "Successful";
                if (!string.IsNullOrEmpty(returnURL))
                {
                    return Redirect(returnURL);
                }
                return RedirectToAction("Index", "Home");
            }

            return View(loginViewModel);

        }


        // Done
        [AllowAnonymous]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Login");
        }


        // Done

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> Category([FromQuery] ProductFiltersViewModel filter)
        {
            if (filter.CategoryId.HasValue)
            {
                var categoryExists = await _context.Category
                    .AnyAsync(c => c.Id == filter.CategoryId && c.Status);

                if (!categoryExists)
                {
                    return Content("Invalid category!");
                }
            }

            var productList = await _context.Product.Where(p => p.IsAvailable && p.InStock > 0 && p.ProductVariants.Any(pv => pv.InStock > 0)).Include(p => p.ProductVariants).Include(p => p.Category).ToListAsync();

            if (filter.CategoryId.HasValue)
            {
                productList = productList.Where(p => p.CategoryId == filter.CategoryId).ToList();
            }

            if (filter.MinPrice.HasValue && filter.MaxPrice.HasValue)
            {

                productList = productList.Where(p => p.ProductVariants.Any(pv => pv.Price >= filter.MinPrice && pv.Price <= filter.MaxPrice)).ToList();
                foreach (var eachProduct in productList)
                {
                    eachProduct.ProductVariants = eachProduct.ProductVariants.Where(pv => pv.InStock > 0 && pv.Price >= filter.MinPrice && pv.Price <= filter.MaxPrice).ToList();
                }
            }

            if (!string.IsNullOrWhiteSpace(filter.SearchQuery))
            {
                productList = productList.Where(p => p.Name.Contains(filter.SearchQuery) || p.Category.Name.Contains(filter.SearchQuery)).ToList();
            }

            switch (filter.OrderBy?.ToLower())
            {
                case "alphabet-asc":
                    productList = productList.OrderBy(p => p.Name).ToList();
                    break;
                case "alphabet-desc":
                    productList = productList.OrderByDescending(p => p.Name).ToList();
                    break;
                default:
                    productList = productList.OrderBy(p => p.Id).ToList();
                    break;
            }

            var user = await _userManager.GetUserAsync(User);

            ViewBag.Categories = await _context.Category.Where(c => c.Status).ToListAsync();
            ViewBag.Filter = filter;
            ViewBag.UserEmail = user == null ? null : user.Email;
            return View(productList);
        }


        // Done
        [AllowAnonymous]
        public async Task<IActionResult> ProductDetail(int id, string size, int quantity)
        {
            Product? product = await _context.Product
                .Include(p => p.Category)
                .Include(p => p.ProductVariants.Where(pv => pv.InStock > 0))
                .FirstOrDefaultAsync(p => p.Id == id);

            if (product == null)
            {
                return NotFound();
            }

            List<Product> relatedProducts = await _context.Product
                .Where(p => p.Category.Name == product.Category.Name && p.ProductVariants.Any(pv => pv.InStock > 0))
                .Include(p => p.ProductVariants.Where(pv => pv.InStock > 0))
                .ToListAsync();


            var user = await _userManager.GetUserAsync(User);
            ViewBag.UserEmail = user == null ? null : user.Email;
            return View(new ProductDetailViewModel(product, size, quantity, relatedProducts, product.ProductVariants));
        }

        //Done
        [Authorize(Roles = "CUSTOMER")]
        public async Task<IActionResult> ShoppingCart(long cartId)
        {

            var cart = await _context.Cart
                .Include(c => c.CartItems)
                    .ThenInclude(ci => ci.Product)
                        .ThenInclude(p => p.ProductVariants)
                .Include(c => c.Customer)
                    .ThenInclude(cus => cus.AddressShippings)
                .Include(c => c.Customer)
                    .ThenInclude(cus => cus.Rank)
                .FirstOrDefaultAsync(c => c.Id == cartId);

            if (cart == null) return NotFound();

            foreach (var ci in cart.CartItems)
            {
                var variant = ci.Product.ProductVariants.FirstOrDefault(pv => pv.Name == ci.ProductSize);
                if (variant != null)
                {
                    if (variant.InStock <= 0)
                    {
                        ci.Status = "Out of Stock";
                    }
                    else if (ci.Quantity > variant.InStock)
                    {
                        ci.Status = "Exceeds Stock";
                    }
                    else
                    {
                        ci.Status = "Available";
                    }
                }
            }

            await _context.SaveChangesAsync();

            var shippingCartDTO = new ShippingCartDTO
            {
                Id = cart.Id,
                cartItems = cart.CartItems.Select(ci =>
                {
                    var variant = ci.Product.ProductVariants.FirstOrDefault(pv => pv.Name == ci.ProductSize);
                    return new ShippingCartItemDTO
                    {
                        Id = ci.Id,
                        FinalPrice = (variant.Price * ci.Quantity) * ((100 - ci.Product.Discount) / 100),
                        ProductDiscount = ci.Product.Discount,
                        ProductSize = ci.ProductSize,
                        Quantity = ci.Quantity,
                        Status = ci.Status,
                        TotalPrice = variant.Price * ci.Quantity,
                        Product = new ShippingProductDTO
                        {
                            Id = ci.Product.Id,
                            Discount = ci.Product.Discount,
                            Image = ci.Product.Image,
                            IsAvailable = ci.Product.IsAvailable,
                            Name = ci.Product.Name,
                            ProductVariant = new ShippingProductVariantDTO
                            {
                                Id = variant.Id,
                                InStock = variant.InStock,
                                Name = variant.Name,
                                Price = variant.Price
                            }
                        }
                    };
                }).ToList(),
                AddressShipping = cart.Customer.AddressShippings.FirstOrDefault(add => add.IsDefaultAddress),
                RankDiscount = cart.Customer.Rank.Discount
            };

            return View(shippingCartDTO);
        }
        [AllowAnonymous]
        public IActionResult AboutUs() => View();

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> Compare(int productId)
        {
            var selectedProduct = await _context.Product
                                          .Include(p => p.Category)
                                          .Include(p => p.ProductVariants.Where(pv => pv.InStock > 0))
                                          .FirstOrDefaultAsync(p => p.Id == productId);

            if (selectedProduct == null)
            {
                return NotFound();
            }

            var relatedProducts = await _context.Product
                                          .Where(p => p.CategoryId == selectedProduct.CategoryId && p.Id != selectedProduct.Id && p.ProductVariants.Any(pv => pv.InStock > 0))
                                          .Include(p => p.ProductVariants.Where(pv => pv.InStock > 0))
                                          .OrderBy(r => Guid.NewGuid())
                                          .Take(3)
                                          .ToListAsync();

            ViewData["SelectedProduct"] = selectedProduct;
            ViewData["RelatedProducts"] = relatedProducts;
            var user = await _userManager.GetUserAsync(User);
            ViewBag.UserEmail = user == null ? null : user.Email;

            return View();
        }

        public IActionResult AccessDenied()
        {
            return View();
        }
    }


}
