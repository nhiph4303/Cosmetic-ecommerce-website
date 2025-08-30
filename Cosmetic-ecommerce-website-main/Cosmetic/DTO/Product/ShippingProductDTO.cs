using Cosmetic.Enums;
using Cosmetic.Models;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Cosmetic.DTO.ProductVariant;

namespace Cosmetic.DTO.Product
{
    public class ShippingProductDTO
    {
        public long Id { get; set; }

        public string Name { get; set; }

        public string Image { get; set; }

        public bool IsAvailable { get; set; }

        public double Discount { get; set; }

        public ShippingProductVariantDTO ProductVariant { get; set; }


        public ShippingProductDTO() { }

        public ShippingProductDTO(long id, string name, string image, bool isAvailable, double discount, ShippingProductVariantDTO productVariant)
        {
            Id = id;
            Name = name;
            Image = image;
            IsAvailable = isAvailable;
            Discount = discount;
            ProductVariant = productVariant;
        }
    }
}
