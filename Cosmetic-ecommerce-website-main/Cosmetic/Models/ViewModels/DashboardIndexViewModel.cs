using System.ComponentModel.DataAnnotations;

namespace Cosmetic.Models.ViewModels
{
    public class DashboardIndexViewModel
    {
        public List<Order> OrderList { get; set; }
        public int TotalProducts { get; set; }
        public int TotalOrder { get; set; }
        public double TotalMonthlyEarning { get; set; }
        public double TotalRevenue { get; set; }

        [DataType(DataType.Date)]
        public DateTime? StartDate { get; set; }

        [DataType(DataType.Date)]
        public DateTime? EndDate { get; set; }

        public string Status { get; set; }

        public int CurrentPage { get; set; }
        public int TotalPages { get; set; }

        public DashboardIndexViewModel() { }

        public DashboardIndexViewModel(
            List<Order> orderList,
            int totalProducts,
            double totalMonthlyEarning,
            double totalRevenue,
            DateTime? startDate,
            DateTime? endDate,
            string status,
            int totalOrder,
            int currentPage = 1,
            int totalPages = 1)
        {
            OrderList = orderList;
            TotalProducts = totalProducts;
            TotalMonthlyEarning = totalMonthlyEarning;
            TotalRevenue = totalRevenue;
            StartDate = startDate;
            EndDate = endDate;
            Status = status;
            TotalOrder = totalOrder;
            CurrentPage = currentPage;
            TotalPages = totalPages;
        }
    }
}
