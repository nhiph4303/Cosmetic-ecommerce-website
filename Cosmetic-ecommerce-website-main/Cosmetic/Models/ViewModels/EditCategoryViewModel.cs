using System.ComponentModel.DataAnnotations;

namespace Cosmetic.Models.ViewModels
{
    public class EditCategoryViewModel
    {

        [Required]
        public long Id { get; set; }

        [Required(ErrorMessage = "Name is required")]
        [StringLength(100, ErrorMessage = "Name must be under 100 characters")]
        public string Name { get; set; }

        [Required(ErrorMessage ="Status is required")]
        public bool Status { get; set; }

        [Required(ErrorMessage = "Description is requried")]
        [StringLength(255, ErrorMessage = "Description must be under 255 characters")]
        public string Description { get; set; }


        [DataType(DataType.Date)]
        public DateTime CreateTime { get; set; }
    }
}
