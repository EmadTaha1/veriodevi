namespace VeriStore.Models
{
    public class CartItem
    {
        public int CartItemId { get; set; }
        public int MusteriId { get; set; }
        public int UrunId { get; set; }
        public string? UrunAdi { get; set; }
        public decimal Fiyat { get; set; }
        public int Quantity { get; set; }
        public decimal ToplamTutar { get; set; }
    }
}
