
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace Shop.Models
{
    public class Order
    {
        [Key]
        public int ID { get; set; }

        [StringLength(50)]

        public String Name { get; set; }

        //public int CustomerID { get; set; }

        [StringLength(255)]

        public String Email { get; set; }

        [Display(Name = "Total Price")]
        public decimal TotalPrice { get; set; }

        public DateTime Date { get; set; }


        public String Status { get; set; }

    }
}
