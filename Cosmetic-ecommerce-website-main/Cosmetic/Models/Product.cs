using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Cosmetic.Enums;

namespace Cosmetic.Models
{
    public class Product
    {
        [Key]
        public long Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public string Image { get; set; }

        [Display(Name = "In Stock")]
        public long InStock { get; set; }

        [Column(TypeName = "nvarchar(24)")]
        public ProductType ProductType { get; set; }

        public bool IsAvailable { get; set; }

        public double Discount { get; set; }

        public long CategoryId { get; set; }

        public Category Category { get; set; }

        public DateTime CreateTime { get; set; }

        public List<CartItem> CartItems { get; set; }

        public List<OrderDetail> OrderDetails { get; set; }

        public List<ProductVariant> ProductVariants { get; set; }

        public Product() { }

        public Product(long id, string name, string description, string image, long inStock, double discount, long categoryId, Category category)
        {
            Name = name;
            Description = description;
            Image = image;
            InStock = inStock;
            IsAvailable = true;
            Discount = discount;
            CategoryId = categoryId;
            Category = category;
            CreateTime = DateTime.Now;
            CartItems = new List<CartItem>();
            OrderDetails = new List<OrderDetail>();
            ProductVariants = new List<ProductVariant>();
        }
    }
}




