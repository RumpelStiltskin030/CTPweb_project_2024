using System;
using System.Collections.Generic;

namespace HomeWorks.Models;

public partial class Product
{
    public string ProId { get; set; } = null!;

    public string MeId { get; set; } = null!;

    public string ProName { get; set; } = null!;

    public decimal ProPrice { get; set; }

    public string Status { get; set; } = null!;

    public virtual Member Me { get; set; } = null!;

    public virtual ICollection<OrderDetail> OrderDetails { get; set; } = new List<OrderDetail>();

    public virtual ICollection<ProductDetail> ProductDetails { get; set; } = new List<ProductDetail>();
}
