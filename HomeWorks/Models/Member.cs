using System;
using System.Collections.Generic;

namespace HomeWorks.Models;

public partial class Member
{
    public string MeId { get; set; } = null!;

    public string MeName { get; set; } = null!;

    public string MeTel { get; set; } = null!;

    public string MeEmail { get; set; } = null!;

    public string MePassword { get; set; } = null!;

    public string PreId { get; set; } = null!;

    public string? PhotoPath { get; set; }

    public virtual ICollection<Order> OrderMes { get; set; } = new List<Order>();

    public virtual ICollection<Order> OrderSeles { get; set; } = new List<Order>();

    public virtual Permission Pre { get; set; } = null!;

    public virtual ICollection<Product> Products { get; set; } = new List<Product>();
}
