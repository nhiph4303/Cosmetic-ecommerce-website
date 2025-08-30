using Cosmetic.Data;
using Cosmetic.Models.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Cosmetic.Controllers
{
    [Authorize(Roles = "ADMIN")]
    public class AdminController : Controller
    {

        private readonly CosmeticContext _context;
        private readonly int _pageSize = 10;
        public AdminController(CosmeticContext context)
        {
            _context = context;
        }
        // GET: Admin
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


            var now = DateTime.Now;
            var allOrders = await _context.Order.Include(o => o.OrderDetails).ToListAsync();
            var totalMonthlyEarning = allOrders.Where(o => o.OrderDate.Month == now.Month && o.OrderDate.Year == now.Year).Sum(o => o.FinalPrice);
            var totalSoldProductQuantity = allOrders.Sum(o => o.OrderDetails.Sum(od => od.Quantity));
            var totalRevenue = allOrders.Sum(o => o.FinalPrice);

            return View(new DashboardIndexViewModel
            {
                CurrentPage = page,
                EndDate = endDate,
                StartDate = startDate,
                OrderList = orders,
                Status = status,
                TotalMonthlyEarning = totalMonthlyEarning,
                TotalOrder = allOrders.Count(),
                TotalPages = totalPages,
                TotalProducts = totalSoldProductQuantity,
                TotalRevenue = totalRevenue,

            });
        }
    }
}
