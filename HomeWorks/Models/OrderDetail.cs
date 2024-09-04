using System;
using System.Collections.Generic;

namespace HomeWorks.Models;

public partial class OrderDetail
{
    public int Id { get; set; }

    public string OrdId { get; set; } = null!;

    public string ProId { get; set; } = null!;

    public string? Specification { get; set; }

    public decimal Pricing { get; set; }

    public virtual Order Ord { get; set; } = null!;

    public virtual Product Pro { get; set; } = null!;
}
