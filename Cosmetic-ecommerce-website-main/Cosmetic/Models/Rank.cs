

using System.ComponentModel.DataAnnotations;

namespace Cosmetic.Models
{
    public class Rank
    {
        [Key]
        public long Id { get; set; }

        public string Name { get; set; }

        public double Discount { get; set; }

        public double RequiredPoint { get; set; }

        public List<Customer> Customers { get; set; }

        public Rank() { }
        public Rank(long id, string name, double discount, double requiredPoint)
        {
            this.Id = id;
            this.Name = name;
            this.Discount = discount;
            this.RequiredPoint = requiredPoint;
        }


    }
}
