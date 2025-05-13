using System;
using System.Collections.Generic;

namespace VeriStore.Models;

public partial class Product
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public string? ImageUrl { get; set; }

    public decimal? Price { get; set; }

    public string? Description { get; set; }
}
