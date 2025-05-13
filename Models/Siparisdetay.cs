using System;
using System.Collections.Generic;

namespace VeriStore.Models;

public partial class Siparisdetay
{
    public int SiparisDetayId { get; set; }

    public int? SiparisId { get; set; }

    public int? UrunId { get; set; }

    public int? Adet { get; set; }

    public decimal? BirimFiyat { get; set; }

    public virtual Sipari? Siparis { get; set; }

    public virtual Urun? Urun { get; set; }
}
