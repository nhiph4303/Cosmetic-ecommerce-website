using Cosmetic.DTO.CartItem;

namespace Cosmetic.DTO.Order
{
    public class PlaceOrderRequest
    {
        public List<CartItemPlaceOrderRequest> CartItemsPlaceOrder { get; set; }
        public double FinalPrice { get; set; }
        public double TotalPrice { get; set; }

        public double TotalDiscount { get; set; }

        public double ProductDiscount { get; set; }

        public double RankDiscount { get; set; }

        public string OrderNote { get; set; }

        public long AddressId { get; set; }
    }
}
