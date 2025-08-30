using System.ComponentModel.DataAnnotations;

namespace Cosmetic.Models
{
    public class ProductVariant
    {
        [Key]
        public long Id { get; set; }

        public long ProductId { get; set; }

        public Product Product { get; set; }

        public double Price { get; set; }

        public int InStock { get; set; }

        public string Name { get; set; } // gram,ml,X,L,M

    }
}
