using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace Shop.Models
{
    public class Customer
    {
        [Key]
        public int ID { get; set; }

        public String Name { get; set; }        

        [EmailAddress]
        public String Email { get; set; }

        public String Password { get; set; }

        public String Address { get; set; }

        [StringLength(10)]
        public String PhoneNumber { get; set; }

        public String Status { get; set; }
    }
}
