using System.ComponentModel.DataAnnotations;

namespace Shop.Models
{
    public class Category
    {
        [Key]
        public int ID { get; set; }

        public string Name { get; set; }
        public string Description { get; set; }

        public String Status { get; set; }

        public DateTime CreateTime { get; set; }

    }
}



