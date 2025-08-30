
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace Cosmetic.Models
{
    public class Order
    {
        [Key]
        public long Id { get; set; }

        public string Name { get; set; }

        public string Province { get; set; }

        public string District { get; set; }

        public string Ward { get; set; }

        public double FinalPrice { get; set; }

        public double TotalPrice { get; set; }

        public double TotalDiscount { get; set; }

        public double ProductDiscount { get; set; }

        public double RankDiscount { get; set; }

        public double LoyalPointEarned { get; set; }

        public string SpecificPlace { get; set; }

        public string PhoneNumber { get; set; }

        public string? Note { get; set; }

        public string OrderStatus { get; set; }

        public DateTime OrderDate { get; set; }

        public long CustomerId { get; set; }

        public Customer Customer { get; set; }

        public List<OrderDetail> OrderDetails { get; set; }

        public Order() { }

        public Order(string province, string district, string ward, double finalPrice, double totalPrice, double totalDiscount, double productDiscount, double rankDiscount, double loyalPointEarned, string specificPlace, string phoneNumber, string? note, long customerId, Customer customer)
        {
            Province = province;
            District = district;
            Ward = ward;
            FinalPrice = finalPrice;
            TotalPrice = totalPrice;
            TotalDiscount = totalDiscount;
            ProductDiscount = productDiscount;
            RankDiscount = rankDiscount;
            LoyalPointEarned = loyalPointEarned;
            SpecificPlace = specificPlace;
            PhoneNumber = phoneNumber;
            Note = note;
            OrderStatus = "PENDING";
            OrderDate = DateTime.Now;
            CustomerId = customerId;
            Customer = customer;
        }
    }
}
