namespace Cosmetic.Models.ViewModels
{
    public class ProductVariantIndexViewModel
    {
        public List<ProductVariant> ProductVariants { get; set; }
        public int CurrentPage { get; set; }
        public int TotalPages { get; set; }
    }
}
