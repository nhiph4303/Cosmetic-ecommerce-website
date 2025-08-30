using Cosmetic.Enums;
using System.ComponentModel.DataAnnotations;

namespace Cosmetic.Models.ViewModels
{
    public class ProductCreateViewModel
    {
        [Required(ErrorMessage = ("Name is required"))]
        public string Name { get; set; }

        [Required(ErrorMessage = ("Description is required"))]
        public string Description { get; set; }

        public string? Image { get; set; }

        [Required(ErrorMessage = ("ProductType is required"))]
        public ProductType ProductType { get; set; }

        [Required(ErrorMessage = ("Status is required"))]
        public bool IsAvailable { get; set; }


        [Required(ErrorMessage = ("Discount is required"))]
        [Range(0, 100, ErrorMessage = ("Discount must be between 0 and 100"))]
        public double Discount { get; set; }

        public long? CategoryId { get; set; }

        public List<Category>? CategoryMenu { get; set; }
    }
}
