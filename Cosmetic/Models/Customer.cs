using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Identity;
namespace Shop.Models
{
    public class Customer : IdentityUser
    {
        [Key]
        public int ID { get; set; }

        public string Name { get; set; }

        [EmailAddress]
        public string Address { get; set; }

        [StringLength(10)]
        public string PhoneNumber { get; set; }

        public string Status { get; set; }
    }
}
