using System;
using System.Collections.Generic;

namespace VeriStore.Models;

public partial class Urun
{
    public int UrunId { get; set; }
    public string? UrunAdi { get; set; }
    public string? Kategori { get; set; }
    public decimal? Fiyat { get; set; }
    public int? StokMiktari { get; set; }
    public string? Aciklama { get; set; }
    public virtual ICollection<Siparisdetay> Siparisdetays { get; set; } = new List<Siparisdetay>();
}
