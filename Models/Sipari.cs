using System;
using System.Collections.Generic;

namespace VeriStore.Models;

public partial class Sipari
{
    public int SiparisId { get; set; }

    public int? MusteriId { get; set; }

    public DateTime? SiparisTarihi { get; set; }

    public decimal? ToplamTutar { get; set; }

    public virtual Musteri? Musteri { get; set; }

    public virtual ICollection<Siparisdetay> Siparisdetays { get; set; } = new List<Siparisdetay>();
}
