using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Identity;

namespace Cosmetic.Models
{
    public class Admin
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

        public string UserId { get; set; }

        public IdentityUser User { get; set; }

        public Admin() { }

        public Admin(string name, string? address, string phoneNumber, DateTime dateOfBirth, bool gender, bool isActive, string userId,IdentityUser user)
        {
            Name = name;
            Address = address;
            PhoneNumber = phoneNumber;
            DateOfBirth = dateOfBirth;
            Gender = gender;
            IsActive = isActive;
            UserId = userId;
            User = user;
        }
    }
}
