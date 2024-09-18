using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace HomeWorks.Models;

public partial class OrderDetail
{
    public int Id { get; set; }

    [Display(Name = "訂單編號")]
    public string OrdId { get; set; } = null!;

    [Display(Name = "產品編號")]
    public string ProId { get; set; } = null!;

    [Display(Name = "規格")]
    public string? Specification { get; set; }

    [Display(Name = "商品訂價")]
    public decimal Pricing { get; set; }

    public virtual Order Ord { get; set; } = null!;

    public virtual Product Pro { get; set; } = null!;

    public string Fieldpath { get; set; } = null!;
}
