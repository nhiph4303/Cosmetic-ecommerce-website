using System.ComponentModel.DataAnnotations;
using Cosmetic.Models;

namespace Cosmetic.Models
{
    public class CartItem
    {
        [Key]
        public long Id { get; set; }

        public string Status { get; set; } // available, out of stock, over in stock quantity

        public int Quantity { get; set; }

        public string ProductSize { get; set; }

        public double FinalPrice { get; set; }

        public double ProductDiscount { get; set; }

        public double TotalPrice { get; set; }

        public long ProductId { get; set; }

        public Product Product { get; set; }

        public long CartId { get; set; }

        public Cart Cart { get; set; }

        public CartItem() { }

        public CartItem(int quantity, double finalPrice, double productDiscount, double totalPrice, string productSize, long productId, Product product, long cartId, Cart cart)
        {
            Status = "Available";
            Quantity = quantity;
            FinalPrice = finalPrice;
            ProductDiscount = productDiscount;
            TotalPrice = totalPrice;
            ProductSize = productSize;
            ProductId = productId;
            Product = product;
            CartId = cartId;
            Cart = cart;
        }
    }
}
