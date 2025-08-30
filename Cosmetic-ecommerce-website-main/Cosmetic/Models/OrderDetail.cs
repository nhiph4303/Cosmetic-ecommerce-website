using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace Cosmetic.Models
{
    public class OrderDetail
    {
        [Key]
        public long Id { get; set; }

        public double FinalPrice { get; set; }

        public double TotalPrice { get; set; }

        public double ProductDiscount { get; set; }

        public int Quantity { get; set; }

        public string ProductSize { get; set; }

        public long OrderId { get; set; }

        public Order Order { get; set; }

        public long ProductId { get; set; }

        public Product Product { get; set; }

        public OrderDetail() { }

        public OrderDetail(double finalPrice, double totalPrice, double productDiscount, int quantity, string productSize, long orderId, Order order, long productId, Product product)
        {
            FinalPrice = finalPrice;
            TotalPrice = totalPrice;
            ProductDiscount = productDiscount;
            Quantity = quantity;
            ProductSize = productSize;
            OrderId = orderId;
            Order = order;
            ProductId = productId;
            Product = product;
        }
    }
}
