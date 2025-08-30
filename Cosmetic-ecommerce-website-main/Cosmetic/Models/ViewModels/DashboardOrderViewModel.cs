using System.ComponentModel.DataAnnotations;

namespace Cosmetic.Models.ViewModels
{
    public class DashboardOrderViewModel
    {
        public List<Order> orderList { get; set; }

        [DataType(DataType.Date)]
        public DateTime? StartDate { get; set; }

        [DataType(DataType.Date)]
        public DateTime? EndDate { get; set; }

        public string Status { get; set; }

        public int CurrentPage { get; set; }
        public int TotalPages { get; set; }

    }
}
