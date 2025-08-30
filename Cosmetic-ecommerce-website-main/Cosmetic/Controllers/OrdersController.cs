using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Cosmetic.Data;
using Cosmetic.Models;
using Cosmetic.DTO.CartItem;
using Cosmetic.DTO.Order;
using Microsoft.CodeAnalysis.Options;
using Newtonsoft.Json.Linq;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;
using Cosmetic.Models.ViewModels;


namespace Cosmetic.Controllers
{
    [Route("Orders")]
    public class OrdersController : Controller
    {
        private readonly CosmeticContext _context;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly int[] RankId = [1, 2, 3, 4 ];
        private readonly double[] RankRequirePoint =  [ 0, 2000, 5000, 7000 ];
        private readonly int _pageSize = 10;

        public OrdersController(CosmeticContext context, UserManager<IdentityUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        [Authorize(Roles = "CUSTOMER,ADMIN")]
        [HttpGet]
        public async Task<IActionResult> Index(string status = "All", DateTime? startDate = null, DateTime? endDate = null, int page = 1)
        {

            var orderQuery = _context.Order.AsQueryable();

            if (!string.IsNullOrEmpty(status) && status != "All")
            {
                orderQuery = orderQuery.Where(o => o.OrderStatus == status);
            }

            if (startDate.HasValue)
            {
                orderQuery = orderQuery.Where(o => o.OrderDate >= startDate.Value);
            }

            if (endDate.HasValue)
            {
                var adjustedEndDate = endDate.Value.AddDays(1).AddSeconds(-1);
                orderQuery = orderQuery.Where(o => o.OrderDate <= adjustedEndDate);
            }

            var totalFilteredOrders = await orderQuery.CountAsync();
            var totalPages = (int)Math.Ceiling(totalFilteredOrders / (double)_pageSize);

            page = Math.Max(1, Math.Min(page, Math.Max(1, totalPages)));

            var orders = await orderQuery
                .OrderByDescending(o => o.Id)
                .Skip((page - 1) * _pageSize)
                .Take(_pageSize)
                .ToListAsync();


            return View(new DashboardOrderViewModel
            {
                CurrentPage = page,
                EndDate = endDate,
                orderList = orders,
                TotalPages = totalPages,
                StartDate = startDate,
                Status = status,
                
            });
        }

        [HttpPost("PlaceOrder")]
        [Authorize(Roles = "CUSTOMER")]
        public async Task<IActionResult> PlaceOrder([FromBody] PlaceOrderRequest request)
        {

            if (!request.CartItemsPlaceOrder.Any() || request.CartItemsPlaceOrder == null || request.CartItemsPlaceOrder.Count() == 0)
            {
                return Json(new
                {
                    success = false,
                    message = "Please pick any items to place order!!"
                });
            }

            long addressId = request.AddressId;
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound();
            }
            Customer customer = await _context.Customer.Include(c => c.AddressShippings.Where(addShip => addShip.Id == addressId)).FirstOrDefaultAsync(c => c.UserId == user.Id);

            if (customer == null)
            {
                return Json(new
                {
                    success = false,
                    message = "Customer not found"
                });
            }

            if (!customer.AddressShippings.Any())
            {
                return Json(new
                {
                    success = false,
                    message = "No default Address Shipping"
                });
            }

            List<long> cartItemIds = request.CartItemsPlaceOrder.Select(ci => ci.Id).ToList();
            List<CartItem> checkedCartItems = await _context.CartItems.Where(ci => cartItemIds.Contains(ci.Id)).Include(ci => ci.Product).ThenInclude(p => p.ProductVariants).ToListAsync();
            List<UnavailableCartItem> unavailableCartItems = new List<UnavailableCartItem>();
            foreach (var eachCartItem in checkedCartItems)
            {
                ProductVariant productVariant = eachCartItem.Product.ProductVariants.Find(pv => pv.Name == eachCartItem.ProductSize);
                if (productVariant.InStock <= 0)
                {
                    unavailableCartItems.Add(new UnavailableCartItem
                    {
                        Id = eachCartItem.Id,
                        Status = "Out of Stock",
                        FinalPrice = eachCartItem.FinalPrice,
                        TotalPrice = eachCartItem.TotalPrice,
                    });
                    eachCartItem.Status = "Out of Stock";
                }
                else if (productVariant.InStock < eachCartItem.Quantity)
                {
                    unavailableCartItems.Add(new UnavailableCartItem
                    {
                        Id = eachCartItem.Id,
                        Status = "Exceeds Stock",
                        RemainQuantity = productVariant.InStock,
                    });
                    eachCartItem.Status = "Exceeds Stock";
                }
            }

            if (unavailableCartItems.Any())
            {
                await _context.SaveChangesAsync();
                return Json(new
                {
                    success = false,
                    message = "Unavailable Cart Items",
                    unavailableCartItems
                });
            }


            AddressShipping defaultAddressShipping = customer.AddressShippings[0];

            Order order = new Order
            {
                Customer = customer,
                CustomerId = customer.Id,
                Name = defaultAddressShipping.Name,
                Province = defaultAddressShipping.Province,
                District = defaultAddressShipping.District,
                Ward = defaultAddressShipping.Ward,
                FinalPrice = request.FinalPrice,
                TotalPrice = request.TotalPrice,
                TotalDiscount = request.TotalDiscount,
                ProductDiscount = request.ProductDiscount,
                RankDiscount = request.RankDiscount,
                LoyalPointEarned = Math.Round(request.FinalPrice / 10, 2, MidpointRounding.AwayFromZero),
                SpecificPlace = defaultAddressShipping.SpecificPlace,
                PhoneNumber = defaultAddressShipping.PhoneNumber,
                Note = request.OrderNote,
                OrderDate = DateTime.Now,
                OrderStatus = "PENDING",
                OrderDetails = new List<OrderDetail>(),
            };

            await _context.Order.AddAsync(order);
            await _context.SaveChangesAsync();



            foreach (CartItem eachCartItem in checkedCartItems)
            {
                Product product = eachCartItem.Product;
                ProductVariant productVariant = product.ProductVariants.FirstOrDefault(pv => pv.Name == eachCartItem.ProductSize);
                if (productVariant == null)
                {
                    continue;
                }
                productVariant.InStock -= eachCartItem.Quantity;
                product.InStock -= eachCartItem.Quantity;
                order.OrderDetails.Add(new OrderDetail
                {
                    Product = product,
                    ProductId = product.Id,
                    Order = order,
                    OrderId = order.Id,
                    ProductDiscount = product.Discount,
                    ProductSize = productVariant.Name,
                    Quantity = eachCartItem.Quantity,
                    TotalPrice = eachCartItem.TotalPrice,
                    FinalPrice = eachCartItem.FinalPrice,
                });
            }

            await _context.SaveChangesAsync();
            return Json(new
            {
                success = true,
                message = "Place Order Successfully!!"
            });
        }

        [HttpPut("ChangeStatusOrder")]
        [Authorize(Roles = "CUSTOMER,ADMIN")]
        public async Task<IActionResult> ChangeStatusOrder([FromBody] ChangeStatusOrderRequest request)
        {
            long id = request.Id;
            string status = request.Status.ToUpper();
            Order order = await _context.Order.Include(o => o.OrderDetails).ThenInclude(od => od.Product).ThenInclude(p => p.ProductVariants).Include(o => o.Customer).ThenInclude(c => c.Rank).FirstOrDefaultAsync(o => o.Id == id);
            if (order == null)
            {
                return NotFound(new
                {
                    success = false,
                    message = "Order not found."
                });
            }

            if (status == "CANCELLED")
            {
                foreach (OrderDetail eachOrderDetail in order.OrderDetails)
                {
                    Product product = eachOrderDetail.Product;
                    ProductVariant productVariant = product.ProductVariants.FirstOrDefault(pv => pv.Name == eachOrderDetail.ProductSize);
                    if (productVariant == null)
                    {
                        continue;
                    }
                    product.InStock += eachOrderDetail.Quantity;
                    productVariant.InStock += eachOrderDetail.Quantity;
                }
            }
            else if (status == "RETURN")
            {
                foreach (OrderDetail eachOrderDetail in order.OrderDetails)
                {
                    Product product = eachOrderDetail.Product;
                    ProductVariant productVariant = product.ProductVariants.FirstOrDefault(pv => pv.Name == eachOrderDetail.ProductSize);
                    if (productVariant == null)
                    {
                        continue;
                    }
                    product.InStock += eachOrderDetail.Quantity;
                    productVariant.InStock += eachOrderDetail.Quantity;
                }

                order.Customer.LoyalPoints -= order.LoyalPointEarned;
                for (int i = RankRequirePoint.Length - 1; i >= 0; i--)
                {
                    if (order.Customer.LoyalPoints >= RankRequirePoint[i])
                    {
                        var getRankId = RankId[i];
                        if (order.Customer.Rank.Id != getRankId)
                        {
                            Rank rank = await _context.Rank.FirstOrDefaultAsync(r => r.Id == getRankId);
                            if (rank == null)
                            {
                                return BadRequest("Rank is not found");
                            }
                            order.Customer.Rank = rank;
                        }
                        break;
                    }
                }
            }
            else if (status == "PENDING")
            {
                foreach (OrderDetail eachOrderDetail in order.OrderDetails)
                {
                    Product product = eachOrderDetail.Product;
                    ProductVariant productVariant = product.ProductVariants.Find(pv => pv.Name == eachOrderDetail.ProductSize);

                    if (productVariant.InStock <= 0 || eachOrderDetail.Quantity > productVariant.InStock)
                    {
                        return Json(new
                        {
                            success = false,
                            message = "Unavailable Products!!"
                        });
                    }
                    eachOrderDetail.TotalPrice = eachOrderDetail.Quantity * productVariant.Price;
                    eachOrderDetail.FinalPrice = eachOrderDetail.TotalPrice * ((100 - product.Discount) / 100);
                    product.InStock -= eachOrderDetail.Quantity;
                    productVariant.InStock -= eachOrderDetail.Quantity;
                }
                order.TotalPrice = order.OrderDetails.Sum(od => od.TotalPrice);
                order.ProductDiscount = order.OrderDetails.Sum(od => (od.TotalPrice - od.FinalPrice));
                order.OrderDate = DateTime.Now;
                order.RankDiscount = (order.TotalPrice - order.ProductDiscount) * order.Customer.Rank.Discount / 100;
                order.TotalDiscount = order.ProductDiscount + order.RankDiscount;
                order.FinalPrice = order.TotalPrice - order.TotalDiscount;
                order.LoyalPointEarned = Math.Round(order.FinalPrice / 10, 2, MidpointRounding.AwayFromZero);
            }
            else if (status == "COMPLETED")
            {
                order.Customer.LoyalPoints += order.LoyalPointEarned;
                for (int i = RankRequirePoint.Length-1;i>=0;i--)
                {
                    if (order.Customer.LoyalPoints >= RankRequirePoint[i])
                    {
                        var getRankId = RankId[i];
                        if (order.Customer.Rank.Id != getRankId)
                        {
                            Rank rank = await _context.Rank.FirstOrDefaultAsync(r => r.Id == getRankId);
                            if(rank == null)
                            {
                                return BadRequest("Rank is not found");
                            }
                            order.Customer.Rank = rank;
                        }
                        break;
                    }
                }
            }
            else if(status != "SHIPPED")
            {
                return Content("Wrong Status");
            }

            order.OrderStatus = status;
            await _context.SaveChangesAsync();
            return Json(new
            {
                success = true,
                message = "Change status successfully!!"
            });
        }


        [HttpGet("OrderDetail")]

        public async Task<IActionResult> OrderDetail(long id)
        {

            var order = await _context.Order
               .Include(o => o.Customer)
               .ThenInclude(c => c.User)
               .Include(o => o.OrderDetails)
               .ThenInclude(od => od.Product)
               .FirstOrDefaultAsync(m => m.Id == id);
            if (order == null)
            {
                return NotFound();
            }

            return View(order);
        }
    }
}
