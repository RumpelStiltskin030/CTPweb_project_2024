using System;
using System.Collections.Generic;

namespace HomeWorks.Models;

public partial class Order
{
    public string OrdId { get; set; } = null!;

    public string MeId { get; set; } = null!;

    public DateTime OrdDate { get; set; }

    public DateTime? DateLine { get; set; }

    public string Status { get; set; } = null!;

    public string SeleId { get; set; } = null!;

    public virtual Member Me { get; set; } = null!;

    public virtual ICollection<OrderDetail> OrderDetails { get; set; } = new List<OrderDetail>();

    public virtual Member Sele { get; set; } = null!;
}
