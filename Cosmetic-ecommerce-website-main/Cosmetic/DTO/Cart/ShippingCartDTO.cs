using Cosmetic.DTO.CartItem;
using Cosmetic.Models;

namespace Cosmetic.DTO.Cart
{
    public class ShippingCartDTO
    {
        public long Id { get; set; }

        public List<ShippingCartItemDTO> cartItems { get; set; }

        public AddressShipping AddressShipping { get; set; }

        public double RankDiscount { get; set; }

        public ShippingCartDTO() { }

        public ShippingCartDTO(long id, List<ShippingCartItemDTO> cartItems, AddressShipping addressShipping)
        {
            Id = id;
            this.cartItems = cartItems;
            AddressShipping = addressShipping;
        }
    }
}
