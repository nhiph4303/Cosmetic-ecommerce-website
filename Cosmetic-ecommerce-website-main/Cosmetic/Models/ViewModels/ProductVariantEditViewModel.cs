using System.ComponentModel.DataAnnotations;

namespace Cosmetic.Models.ViewModels
{
    public class ProductVariantEditViewModel
    {

        [Required(ErrorMessage =("Id is required"))]
        public long Id { get; set; }

        [Required(ErrorMessage =("Name is required"))]
        public string Name { get; set; }

        [Required(ErrorMessage =("Price is required"))]
        [Range(0.000001, double.MaxValue, ErrorMessage = "Value must be positive")]
        public double Price { get; set; }

        [Required(ErrorMessage =("InStock is required"))]
        [Range(0, int.MaxValue, ErrorMessage = "Value must be positive")]
        public int InStock { get; set; }

        
        public Product? Product { get; set; }
    }
}
