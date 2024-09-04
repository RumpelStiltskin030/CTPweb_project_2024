using System;
using System.Collections.Generic;

namespace HomeWorks.Models;

public partial class ProductDetail
{
    public int Id { get; set; }

    public string ProId { get; set; } = null!;

    public string Fieldpath { get; set; } = null!;

    public virtual Product Pro { get; set; } = null!;
}
