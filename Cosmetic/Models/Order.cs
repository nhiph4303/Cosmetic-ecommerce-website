
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace Shop.Models
{
    public class Order
    {
        [Key]
        public int ID { get; set; }

        [ForeignKey("CustomerID")]
        public Customer? Customer { get; set; }

        public int? CustomerID { get; set; }
        
        [Display(Name = "Total Price")]
        public decimal TotalPrice { get; set; }

        public DateTime Date { get; set; }

        public String Status { get; set; }

        public String Note { get; set; }

    }
}
