using System.ComponentModel.DataAnnotations;
using System.Security.Principal;


namespace Cosmetic.Models
{
    public class Cart
    {
        [Key]
        public long Id { get; set; }

        public long CustomerId { get; set; }

        public Customer Customer { get; set; }

        public List<CartItem> CartItems { get; set; }

        public Cart() { }

        public Cart(long customerId, Customer customer)
        {
            this.CustomerId = customerId;
            this.Customer = customer;
            this.CartItems = new List<CartItem>();
        }
    }
}
