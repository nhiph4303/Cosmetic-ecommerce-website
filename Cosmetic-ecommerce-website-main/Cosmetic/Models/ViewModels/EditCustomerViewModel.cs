using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Cosmetic.Models.ViewModels
{
    public class EditCustomerViewModel
    {
        [Required(ErrorMessage = "Name is required")]
        public string Name { get; set; }
        public string Email { get; set; }

        [Required(ErrorMessage = "Date Of Birth is required")]
        [DataType(DataType.Date)]
        [Column(TypeName = "date")]
        public DateTime DoB { get; set; }

        [Required(ErrorMessage = "Gender is required")]
        public bool Gender { get; set; }

        public string? Address { get; set; }

        [Required(ErrorMessage = "Phone Number is required")]
        [StringLength(10, MinimumLength = 10, ErrorMessage = "Phone Number must be exactly 10 digits")]
        [RegularExpression(@"^\d{10}$", ErrorMessage = "Phone Number must be numeric and 10 digits")]
        public string PhoneNumber { get; set; }

    }
}
