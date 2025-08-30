namespace Cosmetic.DTO.CartItem
{
    public class UnavailableCartItem
    {
        public long Id { get; set; }

        public string Status { get; set; }

        public double FinalPrice { get; set; }

        public double TotalPrice { get; set; }

        public int RemainQuantity { get; set; }
    }
}
