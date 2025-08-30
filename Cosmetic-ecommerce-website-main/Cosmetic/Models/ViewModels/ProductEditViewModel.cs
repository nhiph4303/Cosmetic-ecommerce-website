using System.ComponentModel.DataAnnotations;
using Cosmetic.Enums;
using System.ComponentModel.DataAnnotations.Schema;

namespace Cosmetic.Models.ViewModels
{
    public class ProductEditViewModel
    {
        [Required(ErrorMessage =("Id is required"))]
        public long Id { get; set; }

        [Required(ErrorMessage =("Name is required"))]
        public string Name { get; set; }

        [Required(ErrorMessage = ("Description is required"))]
        public string Description { get; set; }

        [Required(ErrorMessage = ("Image is required"))]
        public string Image { get; set; }

        [Required(ErrorMessage = ("InStock is required"))]
        public long InStock { get; set; }

        [Required(ErrorMessage = ("ProductType is required"))]
        public ProductType ProductType { get; set; }

        [Required(ErrorMessage = ("Status is required"))]
        public bool IsAvailable { get; set; }


        [Required(ErrorMessage = ("Discount is required"))]
        [Range(0,100,ErrorMessage =("Discount must be between 0 and 100"))]
        public double Discount { get; set; }

        [Required(ErrorMessage = ("CategoryId is required"))]
        public long CategoryId { get; set; }

        [DataType(DataType.Date)]
        public DateTime? CreateTime { get; set; }

        public List<ProductVariant>? ProductVariants { get; set; }

        public List<Category>? CategoryMenu { get; set; }
    }
}
