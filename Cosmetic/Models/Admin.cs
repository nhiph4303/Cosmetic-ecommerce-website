using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace Shop.Models
{
    public class Admin
    {
        [Key]
        public int ID { get; set; }

        public String Name { get; set; }

        public String Password { get; set; }

        [EmailAddress]
        public String Email { get; set; }

        [StringLength(10)]
        public String PhoneNumber { get; set; }

        public String Status { get; set; }
    }
}
