using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace HomeWorks.Models;

public partial class Product
{
    [Display(Name = "產品編號")]
    public string ProId { get; set; } = null!;

    [Display(Name = "商品持有人")]
    public string MeId { get; set; } = null!;
    [Display(Name = "產品名稱")]
    public string ProName { get; set; } = null!;
    [Display(Name = "產品價格")]
    public decimal ProPrice { get; set; }
    [Display(Name = "產品狀態")]
    public string Status { get; set; } = null!;

    public virtual Member Me { get; set; } = null!;

    public virtual ICollection<OrderDetail> OrderDetails { get; set; } = new List<OrderDetail>();

    public virtual ICollection<ProductDetail> ProductDetails { get; set; } = new List<ProductDetail>();
}
