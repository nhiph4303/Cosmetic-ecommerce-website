using System.ComponentModel.DataAnnotations;

namespace Cosmetic.Models
{
    public class Category
    {
        [Key]
        public long Id { get; set; }

        public string Name { get; set; }
        public string Description { get; set; }

        public bool Status { get; set; }

        public DateTime CreateTime { get; set; }

        public List<Product> Products { get; set; }

        public Category() { }

        public Category(string name, string description)
        {
            Name = name;
            Description = description;
            Status = true;
            CreateTime = DateTime.Now;
            Products = new List<Product>();
        }
    }
}



