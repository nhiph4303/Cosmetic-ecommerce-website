namespace Cosmetic.Models.ViewModels
{
    public class DashboardProductIndexViewModel
    {
        public List<Product> productList { get; set; }

        public int CurrentPage { get; set; }

        public int TotalPages { get; set; }
    }
}
