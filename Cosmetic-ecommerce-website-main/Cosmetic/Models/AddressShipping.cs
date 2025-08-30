using System.ComponentModel.DataAnnotations;

namespace Cosmetic.Models
{
    public class AddressShipping
    {
        [Key]
        public long Id { get; set; }
        public string Name { get; set; }

        public string Province { get; set; }

        public string District { get; set; }

        public string Ward { get; set; }

        [MaxLength(255)]
        public string SpecificPlace { get; set; }

        public string PhoneNumber { get; set; }

        public bool IsDefaultAddress { get; set; }

        public long CustomerId { get; set; }

        public Customer Customer { get; set; }

        public AddressShipping() { }

        public AddressShipping( string name, string province, string district, string ward, string phoneNumber, bool isDefaultAddress,string specifiPlace, long customerId, Customer customer)
        {
            Name = name;
            Province = province;
            District = district;
            Ward = ward;
            PhoneNumber = phoneNumber;
            IsDefaultAddress = isDefaultAddress;
            CustomerId = customerId;
            Customer = customer;
            SpecificPlace = specifiPlace;
        }
    }
}
