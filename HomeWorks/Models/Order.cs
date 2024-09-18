using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace HomeWorks.Models;

public partial class Order
{
    [Display(Name = "訂單編號")]
    public string OrdId { get; set; } = null!;

    [Display(Name = "購買者帳號")]
    public string MeId { get; set; } = null!;

    [Display(Name = "訂單日期")]
    public DateTime OrdDate { get; set; }

    [Display(Name = "寄出時間")]
    public DateTime? DateLine { get; set; }

    [Display(Name = "訂單狀態")]
    public string Status { get; set; } = null!;

    [Display(Name = "販售者帳號")]
    public string SeleId { get; set; } = null!;

    public virtual Member Me { get; set; } = null!;

    public virtual ICollection<OrderDetail> OrderDetails { get; set; } = new List<OrderDetail>();

    public virtual Member Sele { get; set; } = null!;
}
