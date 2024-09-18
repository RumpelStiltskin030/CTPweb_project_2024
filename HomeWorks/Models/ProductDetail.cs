using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace HomeWorks.Models;

public partial class ProductDetail
{
    public int Id { get; set; }

    [Display(Name = "產品編號")]
    public string ProId { get; set; } = null!;

    [Display(Name = "產品圖片")]
    public string Fieldpath { get; set; } = null!;

    public virtual Product Pro { get; set; } = null!;
}
