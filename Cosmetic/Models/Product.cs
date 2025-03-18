using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Shop.Models
{
    public class Product
    {
        [Key] 
        public int ID { get; set; }

        public String? Name { get; set; }

        public String? Description { get; set; }
        public decimal? Price { get; set; }

        public string? Image { get; set; }

        [Display(Name = "In Stock")]
        public int? InStock { get; set; }

        public String? Status { get; set; }

        public int? CategoryID { get; set; }

        [ForeignKey("CategoryID")]
        public Category? Category { get; set; }

        public DateTime? CreateTime { get; set; }


    }
}




