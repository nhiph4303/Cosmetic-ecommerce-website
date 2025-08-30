using System.ComponentModel.DataAnnotations;
using Cosmetic.Enums;

namespace Cosmetic.Models.ViewModels
{
    public class ProductVariantCreateViewModel
    {
        public List<Product>? Products { get; set; }

        [Required(ErrorMessage = ("Name is required"))]
        public string Name { get; set; }

        [Required(ErrorMessage = ("Product is required"))]
        public long ProductId { get; set; }


        [Required(ErrorMessage = ("Price is required"))]
        [Range(0.000001, double.MaxValue, ErrorMessage = "Value must be positive")]
        public double Price { get; set; }


        [Required(ErrorMessage = ("InStock is required"))]
        [Range(0, int.MaxValue, ErrorMessage = "Value must be positive")]
        public int InStock { get; set; }
    }
}
