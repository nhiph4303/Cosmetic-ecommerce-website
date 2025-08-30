using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Cosmetic.Models;
using Microsoft.AspNetCore.Identity;

namespace Cosmetic.Models
{
    public class Customer
    {
        [Key]
        public long Id { get; set; }
        public string Name { get; set; }
        public string? Address { get; set; }

        public string PhoneNumber { get; set; }

        //[DataType(DataType.Date)]
        //[DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        [Column(TypeName = "date")]
        public DateTime DateOfBirth { get; set; }

        public bool Gender { get; set; }
        public bool IsActive { get; set; }

        public double LoyalPoints { get; set; } = 0;

        [Column(TypeName = "date")]
        public DateTime StartDate { get; set; }

        public Cart Cart { get; set; }

        public string UserId { get; set; }

        public IdentityUser User { get; set; }

        public List<Order> Orders { get; set; }

        public long RankId { get; set; }
        public Rank Rank { get; set; }

        public List<AddressShipping> AddressShippings { get; set; }

        public Customer() { }

        public Customer(string name, string? address, string phoneNumber, DateTime dateOfBirth, bool gender, string userId, IdentityUser user, long rankId, Rank rank)
        {
            this.Name = name;
            this.Address = address;
            this.PhoneNumber = phoneNumber;
            this.DateOfBirth = dateOfBirth;
            this.Gender = gender;
            this.IsActive = true;
            this.LoyalPoints = 0.0;
            this.StartDate = DateTime.Today;
            this.UserId = userId;
            this.User = user;
            this.Orders = new List<Order>();
            this.RankId = rankId;
            this.Rank = rank;
            this.AddressShippings = new List<AddressShipping>();
        }
    }
}
