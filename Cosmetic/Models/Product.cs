using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Shop.Models
{
    public class Product
    {
        [Key]
        public int ID { get; set; }

        [StringLength(50)]
        public String Name { get; set; }

        [Column(TypeName = "decimal(10,2")]
        public decimal Price { get; set; }

        [StringLength(255)]
        public string Image { get; set; }

        [Display(Name = "In Stock")]
        public int InStock { get; set; }

        public String Status { get; set; }

        public int? CategoryID { get; set; }
        public virtual Category Category { get; set; }

    }
}




