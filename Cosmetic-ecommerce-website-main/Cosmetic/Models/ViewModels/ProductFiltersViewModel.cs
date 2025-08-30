namespace Cosmetic.Models.ViewModels
{
    public class ProductFiltersViewModel
    {
        public long? CategoryId { get; set; }
        public string OrderBy { get; set; }

        public double? MinPrice { get; set; }

        public double? MaxPrice { get; set; }

        public string SearchQuery { get; set; }
    }
}
