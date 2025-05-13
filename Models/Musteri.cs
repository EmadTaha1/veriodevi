using System;
using System.Collections.Generic;

namespace VeriStore.Models;

public partial class Musteri
{
    public int MusteriId { get; set; }

    public string? Isim { get; set; }

    public string? Soyisim { get; set; }

    public string? Telefon { get; set; }

    public string? Email { get; set; }

    public string? Adres { get; set; }

    public string? Password { get; set; }

    public virtual ICollection<Sipari> Siparis { get; set; } = new List<Sipari>();
}
