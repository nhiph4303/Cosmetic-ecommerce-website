namespace Cosmetic.Models.ViewModels
{
    public class ProductDetailViewModel
    {
        public Product Product { get; set; } = null!;

        public string Size { get; set; }

        public int Quantity { get; set; }

        public List<ProductVariant> Variants { get; set; }

        public List<Product> ListProducts { get; set; } = null!;

        public ProductDetailViewModel(Product product, string size, int quantity, List<Product> ListProducts, List<ProductVariant> variants)
        {
            Product = product;
            Size = size;
            Quantity = quantity;
            this.ListProducts = ListProducts;
            Variants = variants;
        }

    }
}
