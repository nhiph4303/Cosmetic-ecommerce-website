using System.ComponentModel.DataAnnotations;

namespace Cosmetic.Models.ViewModels
{
    public class AddressShippingViewModel
    {

        public long Id { get; set; }
 
        [Required(ErrorMessage ="Name is requried")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Province is requried")]
        public string Province { get; set; }

        [Required(ErrorMessage = "District is requried")]
        public string District { get; set; }

        [Required(ErrorMessage = "Ward is requried")]
        public string Ward { get; set; }

        public string? SpecificPlace { get; set; }

        [Required(ErrorMessage = "PhoneNumber is requried")]
        [StringLength(10, MinimumLength = 10, ErrorMessage = "Phone Number must be exactly 10 digits")]
        [RegularExpression(@"^\d{10}$", ErrorMessage = "Phone Number must be numeric and 10 digits")]
        public string PhoneNumber { get; set; }

        public bool IsDefaultAddress { get; set; }


        public List<AddressShipping>? ListAddressShipping { get; set; }
    }
}
