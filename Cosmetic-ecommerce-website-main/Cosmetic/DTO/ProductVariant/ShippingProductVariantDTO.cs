namespace Cosmetic.DTO.ProductVariant
{
    public class ShippingProductVariantDTO
    {
        public long Id { get; set; }

        public double Price { get; set; }

        public int InStock { get; set; }

        public string Name { get; set; } // gram,ml,X,L,M

        public ShippingProductVariantDTO() { }

        public ShippingProductVariantDTO(long id, double price, int inStock, string name)
        {
            Id = id;
            Price = price;
            InStock = inStock;
            Name = name;
        }
    }
}
