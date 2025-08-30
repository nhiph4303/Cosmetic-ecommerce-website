using Cosmetic.DTO.Product;

namespace Cosmetic.DTO.CartItem
{
    public class ShippingCartItemDTO
    {
        public long Id { get; set; }

        public string Status { get; set; }

        public int Quantity { get; set; }

        public string ProductSize { get; set; }

        public double FinalPrice { get; set; }

        public double ProductDiscount { get; set; }

        public double TotalPrice { get; set; }

        public ShippingProductDTO Product { get; set; }

        public ShippingCartItemDTO() { }

        public ShippingCartItemDTO(long id, string status, int quantity, string productSize, double finalPrice, double productDiscount, double totalPrice, ShippingProductDTO product)
        {
            Id = id;
            Status = status;
            Quantity = quantity;
            ProductSize = productSize;
            FinalPrice = finalPrice;
            ProductDiscount = productDiscount;
            TotalPrice = totalPrice;
            Product = product;
        }
    }
}
